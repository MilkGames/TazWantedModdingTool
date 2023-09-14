using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taz_Wanted_Modding_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            unpackingPlatformBox.SelectedIndex = 0;
            unpackingFileTypeBox.SelectedIndex = 0;
        }

        private void unpackButton_Click(object sender, EventArgs e)
        {
            int    platform = 0; // 0 - windows, 1 - ps2, 2 - xbox, 3 - gcn
            bool   onlyList;
            bool   separateFolder;
            bool   onlyOneArchive;
            bool   oneType;
            int    fileType = 0; // 0 - bmp, 1 - gif, 2 - tga, 3 - lom, 4 - obe, 5 - ttf, 6 - wav, 7 - str+wav
            string quickBMSPath = "\"";
            string scriptPath = "\"";
            string inputPath = "\"";
            string outputPath = "\"";
            string launchOptions = "";
            string unpackCommand;

            // preparing
            // getting information from the form
            switch (unpackingPlatformBox.SelectedIndex)
            {
                case 0: platform = 0; break; // windows
                case 1: platform = 1; break; // ps2
                case 2: platform = 2; break; // xbox
                case 3: platform = 3; break; // gcn
            }

            if (unpackingOnlyListCheck.Checked) onlyList = true;
            else onlyList = false;

            if (unpackingSeparateFolderCheck.Checked) separateFolder = true;
            else separateFolder = false;

            if (unpackingOnlyOneArchiveCheck.Checked) onlyOneArchive = true;
            else onlyOneArchive = false;

            if (unpackingOneTypeCheck.Checked)
            {
                oneType = true;
                switch (unpackingFileTypeBox.SelectedIndex)
                {
                    case 0: fileType = 0; break; // bmp
                    case 1: fileType = 1; break; // gif
                    case 2: fileType = 2; break; // tga
                    case 3: fileType = 3; break; // lom
                    case 4: fileType = 4; break; // obe
                    case 5: fileType = 5; break; // ttf
                    case 6: fileType = 6; break; // wav
                    case 7: fileType = 7; break; // str+wav
                }
            }
            else oneType = false;

            // getting paths
            quickBMSPath += unpackingQuickBMSpath.Text + "\" ";
            scriptPath += unpackingScriptPath.Text + "\" ";
            inputPath += unpackingInputPath.Text;
            if (!onlyOneArchive)
            {
                switch (platform)
                {
                    case 0: inputPath += "\\{}.pc\" "; break;
                    case 1: inputPath += "\\{}.ps2\" "; break;
                    case 2: inputPath += "\\{}.xbp\" "; break;
                    case 3: inputPath += "\\{}.gcp\" "; break;
                }
            }
            else inputPath += "\" ";
            outputPath += unpackingOutputPath.Text + "\"";

            // getting launch options
            if (onlyList) launchOptions += "-l ";
            if (separateFolder) launchOptions += "-d ";
            if (oneType)
            {
                launchOptions += "-f ";
                switch (fileType)
                {
                    case 0: launchOptions += "\"{}.bmp\" "; break;
                    case 1: launchOptions += "\"{}.gif\" "; break;
                    case 2: launchOptions += "\"{}.tga\" "; break;
                    case 3: launchOptions += "\"{}.lom\" "; break;
                    case 4: launchOptions += "\"{}.obe\" "; break;
                    case 5: launchOptions += "\"{}.ttf\" "; break;
                    case 6: launchOptions += "\"{}.wav\" "; break;
                    case 7: launchOptions += "\"{}.str;{}.wav\" "; break;
                }
            }
            
            // unpacking
            unpackCommand = " /K "+ "\"" + quickBMSPath + "-. " + launchOptions + scriptPath + inputPath + outputPath + "\"";
            // it feels like quickbms doesn't care about outputPath if I unpack only one archive
            // and i also include -. because it'll terminate the script if you unpack multiple
            // archives and choose to unpack only one type of files
            statusLabel.Text = unpackCommand;
            Process.Start("cmd.exe", unpackCommand);
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            int    platform = 0; // 0 - windows, 1 - ps2, 2 - xbox, 3 - gcn
            bool   searchInsideOtherFolders; // allow searching inside sub-folders lmao
            int    fileType = 0; // 0 - bmp, 1 - gif, 2 - tga, 3 - lom, 4 - obe, 5 - ttf, 6 - wav, 7 - str+wav
            string inputPath = "";
            string outputPath = "";
            if (0 == 0) // fileType = 0 in the future
            {
                //i'll change it later, i don't have time for it now sadge
                string filesPath = decryptingInputPath.Text;
                IEnumerable<string> allfiles = Directory.EnumerateFiles(filesPath, "*.bmp", SearchOption.AllDirectories);
                foreach (String fileName in allfiles)
                {
                    //open file
                    byte[] gameFile = File.ReadAllBytes(fileName);

                    //header parts
                    byte[] bmpHeaderStart = {
                        0x42, 0x4d, 0x46, 0x20, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x46, 0x00, 0x00, 0x00, 0x38, 0x00,
                        0x00, 0x00
                    };

                    //copy sizes to header
                    byte[] bmpHeaderWidthHeight = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        bmpHeaderWidthHeight[i] = gameFile[i];
                    }

                    byte[] bmpHeaderEnd = {
                                    0x01, 0x00, 0x10, 0x00, 0x03, 0x00,
                        0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x13, 0x0b,
                        0x00, 0x00, 0x13, 0x0b, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7c,
                        0x00, 0x00, 0xe0, 0x03, 0x00, 0x00, 0x1f, 0x00,
                        0x00, 0x00, 0x00, 0x80, 0x00, 0x00
                    };

                    //cut data without header and trash at end
                    //1 pixel = 2 bytes
                    int width = BitConverter.ToInt32(bmpHeaderWidthHeight, 0) * 2;
                    int height = BitConverter.ToInt32(bmpHeaderWidthHeight, 4);

                    byte[] rawData = new byte[width * height];
                    for (int i = 0; i < rawData.Length; i++)
                    {
                        rawData[i] = gameFile[i + 32];
                    }

                    //flip data
                    byte[] convertedData = new byte[rawData.Length];
                    for (int i = 0; i < convertedData.Length; i++)
                    {
                        convertedData[i] = rawData[rawData.Length - 1 - i];
                    }

                    //flip strings + flip words
                    for (int i = 0; i < height; i++)
                    {
                        int start = i * width;
                        for (int j = 0; j < width / 2; j++)
                        {
                            byte temp = convertedData[start + j];
                            convertedData[start + j] = convertedData[start + width - j - 1];
                            convertedData[start + width - j - 1] = temp;
                        }
                    }
                    /*
                    //flip words
                    for (int i = 0; i < convertedData.Length; i+=2)
                    {
                        byte temp = convertedData[i];
                        convertedData[i] = convertedData[i+1];
                        convertedData[i + 1] = temp;
                    }
                    */ // non-important piece of code
                       //add parts
                    int length = bmpHeaderStart.Length + bmpHeaderWidthHeight.Length + bmpHeaderEnd.Length + convertedData.Length;
                    byte[] convertedFile = new byte[length];
                    bmpHeaderStart.CopyTo(convertedFile, 0);
                    bmpHeaderWidthHeight.CopyTo(convertedFile, bmpHeaderStart.Length);
                    bmpHeaderEnd.CopyTo(convertedFile, bmpHeaderStart.Length + bmpHeaderWidthHeight.Length);
                    convertedData.CopyTo(convertedFile, bmpHeaderStart.Length + bmpHeaderWidthHeight.Length + bmpHeaderEnd.Length);

                    File.WriteAllBytes(fileName, convertedFile);
                }
            }
        }
        
        /*
        private void convertGif_Click(object sender, EventArgs e)
        {
            //open dialog
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach (String fileName in openFile.FileNames)
                {
                    //open file
                    byte[] gameFile = File.ReadAllBytes(fileName);

                    //header parts
                    byte[] bmpHeaderStart = {
                        0x42, 0x4d, 0x46, 0x20, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x46, 0x00, 0x00, 0x00, 0x38, 0x00,
                        0x00, 0x00
                    };

                    //copy sizes to header
                    byte[] bmpHeaderWidthHeight = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        bmpHeaderWidthHeight[i] = gameFile[i];
                    }

                    byte[] bmpHeaderEnd = {
                                    0x01, 0x00, 0x10, 0x00, 0x03, 0x00,
                        0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x13, 0x0b,
                        0x00, 0x00, 0x13, 0x0b, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7c,
                        0x00, 0x00, 0xe0, 0x03, 0x00, 0x00, 0x1f, 0x00,
                        0x00, 0x00, 0x00, 0x80, 0x00, 0x00
                    };

                    //copy frames number
                    byte gifFrames = gameFile[15];

                    //flip data + cut old header
                    byte[] convertedData = new byte[gameFile.Length - 28 - (gifFrames * 4)];
                    for (int i = 0; i < convertedData.Length; i++)
                    {
                        convertedData[i] = gameFile[gameFile.Length - 1 - i];
                    }

                    //flip strings + flip words
                    //1 pixel = 2 bytes
                    int width = BitConverter.ToInt32(bmpHeaderWidthHeight, 0) * 2;
                    int height = BitConverter.ToInt32(bmpHeaderWidthHeight, 4);
                    for (int i = 0; i < height * gifFrames; i++)
                    {
                        int start = i * width;
                        for (int j = 0; j < width / 2; j++)
                        {
                            byte temp = convertedData[start + j];
                            convertedData[start + j] = convertedData[start + width - j - 1];
                            convertedData[start + width - j - 1] = temp;
                        }
                    }

                    //copy size
                    //multiplied by 2 earlier in width
                    int frameSize = width * height;

                    //divide pictures
                    for (int i = 0; i < gifFrames; i++)
                    {
                        //add parts
                        int length = bmpHeaderStart.Length + bmpHeaderWidthHeight.Length + bmpHeaderEnd.Length + (frameSize);
                        byte[] convertedFile = new byte[length];
                        bmpHeaderStart.CopyTo(convertedFile, 0);
                        bmpHeaderWidthHeight.CopyTo(convertedFile, bmpHeaderStart.Length);
                        bmpHeaderEnd.CopyTo(convertedFile, bmpHeaderStart.Length + bmpHeaderWidthHeight.Length);
                        byte[] frameData = convertedData.Skip(i * frameSize).Take(frameSize).ToArray();
                        frameData.CopyTo(convertedFile, bmpHeaderStart.Length + bmpHeaderWidthHeight.Length + bmpHeaderEnd.Length);

                        //save file with new name
                        string newname = fileName + (gifFrames - i - 1).ToString() + ".bmp";
                        File.WriteAllBytes(newname, convertedFile);
                        //File.WriteAllBytes(fileName + "_" + (gifFrames-i-1).ToString() + ".bmp", convertedFile);
                    }
                    File.Delete(fileName);
                }
            }
        }
        */
        /*
        private void convertAllGifs_Click(object sender, EventArgs e)
        {
            string filesPath = allFilesPath.Text;
            IEnumerable<string> allfiles = Directory.EnumerateFiles(filesPath, "*.gif", SearchOption.AllDirectories);
            foreach (String fileName in allfiles)
                {
                    //open file
                    byte[] gameFile = File.ReadAllBytes(fileName);

                    //header parts
                    byte[] bmpHeaderStart = {
                        0x42, 0x4d, 0x46, 0x20, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x46, 0x00, 0x00, 0x00, 0x38, 0x00,
                        0x00, 0x00
                    };

                    //copy sizes to header
                    byte[] bmpHeaderWidthHeight = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        bmpHeaderWidthHeight[i] = gameFile[i];
                    }

                    byte[] bmpHeaderEnd = {
                                    0x01, 0x00, 0x10, 0x00, 0x03, 0x00,
                        0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x13, 0x0b,
                        0x00, 0x00, 0x13, 0x0b, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7c,
                        0x00, 0x00, 0xe0, 0x03, 0x00, 0x00, 0x1f, 0x00,
                        0x00, 0x00, 0x00, 0x80, 0x00, 0x00
                    };

                    //copy frames number
                    byte gifFrames = gameFile[15];

                    //flip data + cut old header
                    byte[] convertedData = new byte[gameFile.Length - 28 - (gifFrames * 4)];
                    for (int i = 0; i < convertedData.Length; i++)
                    {
                        convertedData[i] = gameFile[gameFile.Length - 1 - i];
                    }

                    //flip strings + flip words
                    //1 pixel = 2 bytes
                    int width = BitConverter.ToInt32(bmpHeaderWidthHeight, 0) * 2;
                    int height = BitConverter.ToInt32(bmpHeaderWidthHeight, 4);
                    for (int i = 0; i < height * gifFrames; i++)
                    {
                        int start = i * width;
                        for (int j = 0; j < width / 2; j++)
                        {
                            byte temp = convertedData[start + j];
                            convertedData[start + j] = convertedData[start + width - j - 1];
                            convertedData[start + width - j - 1] = temp;
                        }
                    }

                    //copy size
                    //multiplied by 2 earlier in width
                    int frameSize = width * height;

                    //divide pictures
                    for (int i = 0; i < gifFrames; i++)
                    {
                        //add parts
                        int length = bmpHeaderStart.Length + bmpHeaderWidthHeight.Length + bmpHeaderEnd.Length + (frameSize);
                        byte[] convertedFile = new byte[length];
                        bmpHeaderStart.CopyTo(convertedFile, 0);
                        bmpHeaderWidthHeight.CopyTo(convertedFile, bmpHeaderStart.Length);
                        bmpHeaderEnd.CopyTo(convertedFile, bmpHeaderStart.Length + bmpHeaderWidthHeight.Length);
                        byte[] frameData = convertedData.Skip(i * frameSize).Take(frameSize).ToArray();
                        frameData.CopyTo(convertedFile, bmpHeaderStart.Length + bmpHeaderWidthHeight.Length + bmpHeaderEnd.Length);

                        //save file with new name
                        string newname = fileName + (gifFrames - i - 1).ToString() + ".bmp";
                        File.WriteAllBytes(newname, convertedFile);
                        //File.WriteAllBytes(fileName + "_" + (gifFrames-i-1).ToString() + ".bmp", convertedFile);
                    }
                    File.Delete(fileName);
            }
        }
        */
        /*
        private void convertTga_Click(object sender, EventArgs e)
        {
            //open open and save dialogs
            if (openFile.ShowDialog() == DialogResult.OK && saveFile.ShowDialog() == DialogResult.OK)
            {
                foreach (String fileName in openFile.FileNames)
                {
                    //open file
                    byte[] gameFile = File.ReadAllBytes(fileName);

                    //if bitmap
                    if (gameFile[0] == 0x42 && gameFile[1] == 0x4D)
                    {
                        //save file with new name
                        string newbmpname = Path.GetDirectoryName(saveFile.FileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".bmp";
                        File.WriteAllBytes(newbmpname, gameFile);
                        continue;
                    }

                    //header part
                    byte[] tgaHeader = {
                        0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x28
                    };

                    //copy sizes to header
                    tgaHeader[12] = gameFile[0];
                    tgaHeader[13] = gameFile[1];
                    tgaHeader[14] = gameFile[4];
                    tgaHeader[15] = gameFile[5];

                    int colorDepth;
                    //copy color depth
                    if (gameFile[8] == 1)
                    {
                        colorDepth = 32;
                        tgaHeader[16] = 32;
                    }
                    else if (gameFile[8] == 3)
                    {
                        colorDepth = 16;
                        tgaHeader[16] = 16;
                    }
                    else
                    {
                        continue;
                    }

                    //footer part
                    byte[] tgaFooter = {
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x54, 0x52, 0x55, 0x45, 0x56, 0x49, 0x53, 0x49,
                        0x4f, 0x4e, 0x2d, 0x58, 0x46, 0x49, 0x4c, 0x45,
                        0x2e, 0x00

                    };

                    //cut data without header and trash at end
                    //1 pixel = (colorDepth/8) bytes
                    int width = BitConverter.ToInt16(tgaHeader, 12) * (colorDepth / 8); //check in header
                    int height = BitConverter.ToInt16(tgaHeader, 14);

                    byte[] rawData = new byte[width * height];
                    for (int i = 0; i < rawData.Length; i++)
                    {
                        rawData[i] = gameFile[i + 20];
                    }

                    //flip data
                    //byte[] convertedData = new byte[rawData.Length];
                    /*
                    for (int i = 0; i < convertedData.Length; i++)
                    {
                        convertedData[i] = rawData[rawData.Length - 1 - i];
                    }
                    //flip strings + flip words
                    for (int i = 0; i < height; i++)
                    {
                        int start = i * width;
                        for (int j = 0; j < width / 2; j++)
                        {
                            byte temp = convertedData[start + j];
                            convertedData[start + j] = convertedData[start + width - j - 1];
                            convertedData[start + width - j - 1] = temp;
                        }
                    }
                    */
        /*
        //flip words
        for (int i = 0; i < convertedData.Length; i+=2)
        {
            byte temp = convertedData[i];
            convertedData[i] = convertedData[i+1];
            convertedData[i + 1] = temp;
        }
        //add * / in the future
        //add parts
        int length = tgaHeader.Length + rawData.Length + tgaFooter.Length;
        byte[] convertedFile = new byte[length];
        tgaHeader.CopyTo(convertedFile, 0);
        rawData.CopyTo(convertedFile, tgaHeader.Length);
        tgaFooter.CopyTo(convertedFile, tgaHeader.Length + rawData.Length);

        //save file with new name
        string newname = Path.GetDirectoryName(saveFile.FileName) + "\\" + Path.GetFileName(fileName)/* + ".tga"*/ // ; missed
        /* File.WriteAllBytes(newname, convertedFile); uncomment if needed
    }
}
}
/*
private void convertWav_Click(object sender, EventArgs e)
{
//open open and save dialogs
if (openFile.ShowDialog() == DialogResult.OK && saveFile.ShowDialog() == DialogResult.OK)
{

    //temp file (.raw in end is required for sox)
    string tempfile = Path.GetDirectoryName(saveFile.FileName) + "\\" + "stream.raw";

    foreach (String fileName in openFile.FileNames)
    {
        //open file
        byte[] gameFile = File.ReadAllBytes(fileName);
        byte[] rawStream = new byte[gameFile.Length - 32];
        Array.Copy(gameFile, 16, rawStream, 0, rawStream.Length);
        //create temp file
        File.WriteAllBytes(tempfile, rawStream);

        //get rate
        UInt32 BitRate = BitConverter.ToUInt32(gameFile, 4);

        //SoX
        ProcessStartInfo SoxInfo = new ProcessStartInfo(SoXpath.Text);
        SoxInfo.Arguments = "-r " + BitRate.ToString() + " -e signed-integer -b 16 -c 1 " + "\"" + tempfile + "\"" + " " + "\"" + Path.Combine(Path.GetDirectoryName(saveFile.FileName), Path.GetFileName(fileName)) + "\"";
        SoxInfo.WindowStyle = ProcessWindowStyle.Hidden;
        var SoxProc = Process.Start(SoxInfo);

        //Wait
        SoxProc.WaitForExit();

    }
    File.Delete(tempfile);
}
}
*/
        /*
        private void ps2bmp_Click(object sender, EventArgs e)
        {
            //open dialog
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach (String fileName in openFile.FileNames)
                {
                    // Load file
                    byte[] gameFile = File.ReadAllBytes(fileName);

                    // Parse dimensions
                    int width = BitConverter.ToInt16(gameFile, 0x40);
                    int height = BitConverter.ToInt16(gameFile, 0x42);
                    int format = BitConverter.ToInt16(gameFile, 0x44);

                    if (format == 0x04) // 16 bit
                    {
                        // Bitmap slice
                        byte[] RawBitmap = new ArraySegment<byte>(gameFile, 0xC0, 2 * width * height).ToArray();

                        // Flip
                        RawBitmap = Flip(RawBitmap, width, height);
                        // Swap
                        RawBitmap = Swap(RawBitmap, width, height);

                        // Color
                        RawBitmap = Color(RawBitmap, width, height);

                        // Uncomment to pack
                        // Swap
                        //RawBitmap = Swap(RawBitmap, width, height);
                        // Flip
                        //RawBitmap = Flip(RawBitmap, width, height);

                        SaveAsBmp(fileName, RawBitmap, width, height);
                    }
                    else if (format == 0x06) // 8 bit with palette
                    {
                        // Bitmap slice
                        byte[] RawBitmap = new ArraySegment<byte>(gameFile, 0xC0, width * height).ToArray();

                        // Palette slice
                        byte[] RawPalette = new ArraySegment<byte>(gameFile, 0xC0 + RawBitmap.Length, 256 * 2).ToArray();

                        // Swap
                        //RawPalette = Swap(RawPalette, 256, 1);

                        byte[] NewBitmap = PaletteTo555(RawBitmap, RawPalette, width, height);

                        // Flip
                        NewBitmap = Flip(NewBitmap, width, height);


                        // Swap
                        //NewBitmap = Swap(NewBitmap, width, height);

                        NewBitmap = Mirror(NewBitmap, width, height);

                        // Color
                        //NewBitmap = Color(NewBitmap, width, height);

                        SaveAsBmp(fileName, NewBitmap, width, height);

                    }
                    else if (format == 0x08) // 4 bit
                    {
                        // Bitmap slice
                        byte[] RawBitmap = new ArraySegment<byte>(gameFile, 0xC0, width * height / 2).ToArray();

                        // Palette slice
                        byte[] RawPalette = new ArraySegment<byte>(gameFile, 0xC0 + width * height / 2, 32).ToArray();

                        // Swap
                        //RawPalette = Swap(RawPalette, 256, 1);

                        byte[] NewBitmap = PaletteTo555(RawBitmap, RawPalette, width, height);

                        SaveAsBmp(fileName, NewBitmap, width, height);
                    }
                    else
                        continue;
                }
            }
        }
        */

        public void SaveAsBmp(string filename, byte[] Bitmap, int width, int height)
        {
            // Init Bmp Header
            byte[] bmpHeader555 = {
                        0x42, 0x4d, 0x46, 0x20, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x46, 0x00, 0x00, 0x00, 0x38, 0x00,
                        0x00, 0x00, 0xDE, 0xAD, 0xBE, 0xEF, 0x13, 0x37,
                        0x13, 0x37, 0x01, 0x00, 0x10, 0x00, 0x03, 0x00,
                        0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x13, 0x0b,
                        0x00, 0x00, 0x13, 0x0b, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7c,
                        0x00, 0x00, 0xe0, 0x03, 0x00, 0x00, 0x1f, 0x00,
                        0x00, 0x00, 0x00, 0x80, 0x00, 0x00
            };
            Array.Copy(BitConverter.GetBytes(width), 0x00, bmpHeader555, 0x12, 0x04);
            Array.Copy(BitConverter.GetBytes(height), 0x00, bmpHeader555, 0x16, 0x04);

            // Append with bitmap
            byte[] BmpFile = new byte[bmpHeader555.Length + Bitmap.Length];
            bmpHeader555.CopyTo(BmpFile, 0);
            Bitmap.CopyTo(BmpFile, bmpHeader555.Length);

            // Save file
            string SaveDir = Path.GetDirectoryName(filename);
            SaveDir = Directory.GetParent(SaveDir).ToString();
            //string SavePath = Path.Combine(SaveDir, Path.GetFileName(filename) )

            string BmpName = Path.Combine(SaveDir, Path.GetFileName(filename));
            File.WriteAllBytes(BmpName, BmpFile);
        }

        public void SaveAsIndexedBmp(string filename, byte[] Bitmap, byte[] Palette, int width, int height)
        {
            // Init Bmp Header
            byte[] bmpHeader256 = {
                0x42, 0x4D, 0x36, 0x08, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x36, 0x04, 0x00, 0x00, 0x28, 0x00,
                0x00, 0x00, 0xDE, 0xAD, 0xBE, 0xEF, 0x13, 0x37,
                0x13, 0x37, 0x01, 0x00, 0x08, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x13, 0x0B,
                0x00, 0x00, 0x13, 0x0B, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            Array.Copy(BitConverter.GetBytes(width), 0x00, bmpHeader256, 0x12, 0x04);
            Array.Copy(BitConverter.GetBytes(height), 0x00, bmpHeader256, 0x16, 0x04);





            // Append with palette and bitmap
            byte[] BmpFile = new byte[bmpHeader256.Length + Palette.Length + Bitmap.Length];
            bmpHeader256.CopyTo(BmpFile, 0);
            Palette.CopyTo(BmpFile, bmpHeader256.Length);
            Bitmap.CopyTo(BmpFile, bmpHeader256.Length + Palette.Length);

            // Save file to parent dir
            string SaveDir = Path.GetDirectoryName(filename);
            SaveDir = Directory.GetParent(SaveDir).ToString();
            string BmpName = Path.Combine(SaveDir, Path.GetFileName(filename));
            File.WriteAllBytes(BmpName, BmpFile);
        }
        /*
        public void SaveAsTga(string filename, byte[] Bitmap, byte[] Palette, int width, int height)
        {
            // Init Tga Header
            byte[] indexedtgaheader = {
                        0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x01, 0x18,
                        0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x20, 0x00,
                        0x08, 0x00
            };
            Array.Copy(BitConverter.GetBytes((UInt16)width), 0x00, indexedtgaheader, 0x0C, 0x02);
            Array.Copy(BitConverter.GetBytes((UInt16)height), 0x00, indexedtgaheader, 0x0E, 0x02);

            // Append with palette and bitmap
            byte[] TgaFile = new byte[indexedtgaheader.Length + Palette.Length + Bitmap.Length];
            indexedtgaheader.CopyTo(TgaFile, 0);
            Palette.CopyTo(TgaFile, indexedtgaheader.Length);
            Bitmap.CopyTo(TgaFile, indexedtgaheader.Length + Palette.Length);

            // Save file
            string TgaName = Path.Combine(Path.GetDirectoryName(saveFile.FileName), Path.GetFileName(filename) + ".tga");
            File.WriteAllBytes(TgaName, TgaFile);
        }
        */
        public byte[] Flip(byte[] Bitmap, int width, int height)
        {
            int size = 2 * width * height;
            byte[] Flipped = new byte[size];
            for (int i = 0; i < size; i++)
            {
                Flipped[i] = (byte)Bitmap[size - i - 1];
            }
            return Flipped;
        }

        // Swap byte pairs (words)
        public byte[] Swap(byte[] Bitmap, int width, int height)
        {
            int size = 2 * width * height;
            byte[] Swapped = new byte[size];
            for (int i = 0; i < size; i += 2)
            {
                byte temp = Bitmap[i];
                Swapped[i] = Bitmap[i + 1];
                Swapped[i + 1] = temp;
            }
            return Swapped;
        }

        // Mirror
        public byte[] Mirror(byte[] Bitmap, int width, int height)
        {
            int size = 2 * width * height;
            byte[] Mirrored = new byte[size];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte temp1 = Bitmap[i*width*2 + width*2 - j*2 - 2];
                    byte temp2 = Bitmap[i * width * 2 + width * 2 - j * 2 - 1];
                    Mirrored[i * width * 2 + width * 2 - j * 2 - 2] = Bitmap[i * width * 2 + j * 2];
                    Mirrored[i * width * 2 + width * 2 - j * 2 - 1] = Bitmap[i * width * 2 + j * 2 + 1];
                    Mirrored[i * width * 2 + j * 2] = temp1;
                    Mirrored[i * width * 2 + j * 2 + 1] = temp2;
                }
            }
            return Mirrored;
        }

        // Mirror Indexed
        public byte[] Mirror256(byte[] Bitmap, int width, int height)
        {
            int size = width * height;
            byte[] Mirrored = new byte[size];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte temp1 = Bitmap[i * width + width - j - 1];
                    Mirrored[i * width + width - j - 1] = Bitmap[i * width + j];
                    Mirrored[i * width + j] = temp1;
                }
            }
            return Mirrored;
        }

        // Color fix (PS2) swap blue and red
        public byte[] Color(byte[] Bitmap, int width, int height)
        {
            int size = 2 * width * height;
            byte[] Colored = new byte[size];
            for (int i = 0; i < size; i += 2)
            {
                // Get blue channel
                byte BlueChannel = Bitmap[i];
                BlueChannel &= 0x1F;
                BlueChannel <<= 2;

                // Get red channel
                byte RedChannel = Bitmap[i + 1];
                RedChannel >>= 2;
                RedChannel &= 0x1F;

                // Replace Blue with Red
                Colored[i] = Bitmap[i];
                Colored[i] &= 0xE0;
                Colored[i] |= RedChannel;

                // Replace Red with Blue
                Colored[i + 1] = Bitmap[i + 1];
                Colored[i + 1] &= 0x83;
                Colored[i + 1] |= BlueChannel;

            }
            return Colored;
        }
        public byte[] Replace(byte[] Bitmap, int width, int height)
        {
            int size = 2 * width * height;
            byte[] Replaced = new byte[size];
            for (int i = 0; i < size; i += 4)
            {
                Replaced[i+2] = (byte)Bitmap[size - i - 2];
                Replaced[i+3] = (byte)Bitmap[size - i - 1];
            }

            return Replaced;
        }
        /*
        private void xbpbmp_Click(object sender, EventArgs e)
        {
            //open open and save dialogs
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach (String fileName in openFile.FileNames)
                {
                    // Load file
                    byte[] gameFile = File.ReadAllBytes(fileName);

                    // Parse dimensions
                    int width = BitConverter.ToInt16(gameFile, 0x2E);
                    int height = BitConverter.ToInt16(gameFile, 0x30);
                    int format = BitConverter.ToInt16(gameFile, 0x2C);
                    if (format == 0x03) // 16 bit
                    {
                        // Bitmap slice
                        byte[] RawBitmap = new ArraySegment<byte>(gameFile, 0x100, 2 * width * height).ToArray();

                        // Shuffle
                        RawBitmap = ShuffleX32(RawBitmap, width, height);

                        // Uncomment to pack
                        // Swap
                        //RawBitmap = Swap(RawBitmap, width, height);
                        // Flip
                        //RawBitmap = Flip(RawBitmap, width, height);
                        // Mirror
                        //RawBitmap = Mirror(RawBitmap, width, height);


                        SaveAsBmp(fileName, RawBitmap, width, height);
                    }
                    else if (format == 0x05 && (width >= 16 || height >= 16)) // Indexed 8 bit
                    {
                        // Palette slice
                        byte[] RawPalette = new ArraySegment<byte>(gameFile, 0x100, 256 * 4).ToArray();
                        // Bitmap slice
                        byte[] RawBitmap = new ArraySegment<byte>(gameFile, 0x100 + (256 * 4), width * height).ToArray();

                        // Shuffle
                        RawBitmap = ShuffleIndexedX32(RawBitmap, width, height);

                        // "Fix" 64x32
                        if (width == 64 && height == 32)
                            RawBitmap = Fix64(RawBitmap, width, height);

                        // Uncomment to pack (format 5)
                        // Flip
                        //RawBitmap = Flip(RawBitmap, width, height/2);
                        // Mirror
                        //RawBitmap = Mirror256(RawBitmap, width, height);

                        SaveAsIndexedBmp(fileName, RawBitmap, RawPalette, width, height);
                    }
                }
            }
        }
        */
        /*
        private void gcpbmp_Click(object sender, EventArgs e)
        {
            //open open and save dialogs
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach (String fileName in openFile.FileNames)
                {
                    // Load file
                    byte[] gameFile = File.ReadAllBytes(fileName);

                    // Parse dimensions
                    int width = BitConverter.ToInt32(gameFile.Skip(0x20).Take(4).Reverse().ToArray(), 0x00);
                    int height = BitConverter.ToInt32(gameFile.Skip(0x24).Take(4).Reverse().ToArray(), 0x00);
                    int format = BitConverter.ToInt32(gameFile.Skip(0x28).Take(4).Reverse().ToArray(), 0x00);
                    if (format == 0x10) // 16 bit
                    {
                        // Bitmap slice
                        byte[] RawBitmap = new ArraySegment<byte>(gameFile, 0xA0, 2 * width * height).ToArray();

                        // Flip
                        RawBitmap = Flip(RawBitmap, width, height);
                        // Swap
                        RawBitmap = Replace(RawBitmap, width, height);

                        // Color
                        //RawBitmap = Color(RawBitmap, width, height);

                        // Swap
                        //RawBitmap = Swap(RawBitmap, width, height);
                        // Flip
                        //RawBitmap = Flip(RawBitmap, width, height);


                        RawBitmap = ShuffleX32(RawBitmap, width, height);

                        SaveAsBmp(fileName, RawBitmap, width, height);
                    }
                    else if (format == 0x12)
                    {
                        continue;
                    }
                }
            }
        }
        */
        /*
        private void findoffsets_Click(object sender, EventArgs e)
        {
            //open open and save dialogs
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // Init
                List<List<int>> Offsets = new List<List<int>>();
                //List<int> OffsetsList = new List<int>();
                bool FirstFileFlag = true;

                // Count
                List<List<int>> Counts = new List<List<int>>();
                //List<int> CountList = new List<int>();

                // Ind
                int Start = 0;
                //int End = 2048;
                int End = 8192;

                foreach (String fileName in openFile.FileNames)
                {
                    // Load file (pc bmp)
                    byte[] gameFile = File.ReadAllBytes(fileName);
                    // Load file 2 (xbp bmp)
                    byte[] gameFile2 = File.ReadAllBytes(fileName.Replace("\\pc\\","\\xbp\\"));
                    // Parse
                    byte[] bitmapPC = gameFile.Skip(0x46).ToArray();
                    byte[] bitmapUnknown = gameFile2.Skip(0x46).ToArray();
                    int size = bitmapPC.Length;


                    // First file
                    if (FirstFileFlag)
                    {
                        for (int i = Start; i < End; i+=2)
                        {
                            List<int> OffsetsList = new List<int>();
                            List<int> CountList = new List<int>();
                            for (int j = 0; j < size-1; j+=2)
                            {
                                // Compare words
                                if (bitmapUnknown[j] == bitmapPC[i] && bitmapUnknown[j+1] == bitmapPC[i+1])
                                {
                                    OffsetsList = OffsetsList.Append(j).ToList();
                                    CountList = CountList.Append(1).ToList();
                                }
                            }
                            Offsets = Offsets.Append(OffsetsList).ToList();
                            Counts = Counts.Append(CountList).ToList();
                        }
                        FirstFileFlag = false;
                    }
                    // Other files
                    else
                    {
                        //ListIndex = 0;
                        for (int i = Start; i < End; i+=2)
                        {
                            int j = 0;
                            foreach (int offset in Offsets[i/2])
                            {
                                if (bitmapUnknown[offset] == bitmapPC[i] && bitmapUnknown[offset+1] == bitmapPC[i+1])
                                    Counts[i / 2][j / 2] += 1;
                                j+=2;
                            }
                        }
                    }
                }

                int Prev = 0;

                // Print offsets or deltas
                for (int i = Start; i < End; i += 2)
                {
                    int Max = Counts[i / 2].Max();
                    int MaxInd = Counts[i / 2].IndexOf(Max);

                    int Current = Offsets[i / 2][MaxInd];
                    int Delta =  Prev - Current;

                    // Uncomment for deltas
                    
                    if (Delta < 0)
                        richTextBox1.Text += '-' + (-Delta).ToString("X4").Substring(1, 3);
                    else
                        richTextBox1.Text += Delta.ToString("X4");

                    richTextBox1.Text += "\t";
                    

                    // Uncomment for offsets
                    //richTextBox1.Text += Current.ToString("X4") + " " ;
                    //richTextBox1.Text += "\n";



                    Prev = Current;

                }
            }
        }
        */
        public byte[] ShuffleX32(byte[] Bitmap, int width, int height)
        {
            int Pos = 0x2AA;
            int[] Vertical = { 
                0x556, 0x2AE,
                0x2B6, 0x2AE,
                0x2D6, 0x2AE,
                0x2B6, 0x2AE,
                0x356, 0x2AE,
                0x2B6, 0x2AE,
                0x2D6, 0x2AE,
                0x2B6, 0x2AE,
            };

            int[] Horizontal = {
                0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02, 0x56, 0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02,
                0x156,
                0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02, 0x56, 0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02
            };

            int size = 2 * width * height;
            byte[] Shuffled = new byte[size];
            for (int i = 0; i < Bitmap.Length/64; i++)
            {
                int VerticalIndex = (i%64)%16;
                Pos -= Vertical[VerticalIndex];
                // Wrap
                if (Pos < 0x0)
                    Pos = Bitmap.Length + Pos;

                Shuffled[(64*i)] = (byte)Bitmap[Pos];
                Shuffled[(64 * i) + 1] = (byte)Bitmap[Pos+1];

                for (int j = 0; j < Horizontal.Length; j++)
                {
                    Pos += Horizontal[j];
                    // Wrap
                    //if (Pos > 0x800)
                    //    Pos -= 0x800;
                    int test0 = (64 * i);
                    int test = (j * 2) + 2 + (64 * i);
                    Shuffled[(j*2) + 2 + (64 * i)] = (byte)Bitmap[Pos];
                    Shuffled[(j*2) + 2 + (64 * i) + 1] = (byte)Bitmap[Pos+1];
                }
            }

            return Shuffled;
        }


        public byte[] ShuffleIndexedX32(byte[] Bitmap, int width, int height)
        {
            int Pos = (0x2AA / 2);// -2;
            int[] Vertical = {
                0x556, 0x2AE,
                0x2B6, 0x2AE,
                0x2D6, 0x2AE,
                0x2B6, 0x2AE,
                0x356, 0x2AE,
                0x2B6, 0x2AE,
                0x2D6, 0x2AE,
                0x2B6, 0x2AE,
            };

            int[] Horizontal = {
                0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02, 0x56, 0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02,
                0x156,
                0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02, 0x56, 0x02, 0x06, 0x02, 0x16, 0x02, 0x06, 0x02
            };

            int size = width * height;
            byte[] Shuffled = new byte[size];
            for (int i = 0; i < Bitmap.Length / 32; i++)
            {
                int VerticalIndex = (i % 32) % 16;
                Pos -= Vertical[VerticalIndex]/2;
                // Wrap
                if (Pos < 0x0)
                    Pos = Bitmap.Length + Pos;
                if (Pos < 0x0)
                    Pos = Bitmap.Length + Pos;
                if (Pos < 0x0)
                    Pos = Bitmap.Length + Pos;
                Shuffled[(32 * i)] = (byte)Bitmap[Pos];
                //Shuffled[(64/2 * i) + 1] = (byte)Bitmap[Pos + 1];

                for (int j = 0; j < Horizontal.Length; j++)
                {
                    Pos += Horizontal[j]/2;
                    // Wrap
                    if (Pos >= Bitmap.Length)
                        Pos -= Bitmap.Length;
                    if (Pos >= Bitmap.Length)
                        Pos -= Bitmap.Length;
                    if (Pos >= Bitmap.Length)
                        Pos -= Bitmap.Length;
                    int test0 = (32 * i);
                    int test = j + 1 + (32 * i);
                    Shuffled[j + 1 + (32 * i)] = (byte)Bitmap[Pos];
                    //Shuffled[(j * 2) + 2 + (64 * i) + 1] = (byte)Bitmap[Pos + 1];
                }
            }

            return Shuffled;
        }

        public byte[] Fix64(byte[] Bitmap, int width, int height)
        {
            int size = width * height;
            byte[] Fixed = new byte[size];

            for (int i = 0; i < height; i+=2)
            {
                for (int j = 0; j < 32; j++)
                {
                    Fixed[i * 64 + j] = Bitmap[((i / 2)+16) * 64 + j];
                    Fixed[i * 64 + j + 64] = Bitmap[((i / 2) + 16) * 64 + j + 32];

                    Fixed[i * 64 + j + 32] = Bitmap[i / 2 * 64 + j];
                    Fixed[i * 64 + j + 64 + 32] = Bitmap[i / 2 * 64 + j + 32];
                }
            }
            return Fixed;
        }

        public byte[] PaletteTo555(byte[] Bitmap, byte[] Palette, int width, int height)
        {
            int size = 2 * width * height;
            byte[] Converted = new byte[size];
            for (int i = 0; i < size/2; i++)
            {
                /*if (Bitmap[i] >= 0x80) 
                {
                    Converted[i * 2] = 0xFF;
                    Converted[(i * 2) + 1] = 0xFF;
                }*/
                Converted[i*2] = (byte)Palette[Bitmap[i] * 2];
                Converted[(i * 2) +1] = (byte)Palette[(Bitmap[i]*2)+1];
            }
            return Converted;
        }

        public byte[] PaletteTo1555(byte[] Bitmap, byte[] Palette, int width, int height)
        {
            int size = 2 * width * height;
            byte[] Converted = new byte[size];
            for (int i = 0; i < size / 2; i++)
            {
                /*if (Bitmap[i] >= 0x80) 
                {
                    Converted[i * 2] = 0xFF;
                    Converted[(i * 2) + 1] = 0xFF;
                }*/
                byte temp = (byte)Palette[Bitmap[i] % 32];
                Converted[i * 2] = temp;
                Converted[(i * 2) + 1] = (byte)Palette[(Bitmap[i] % 32) + 1];
            }
            return Converted;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://aluigi.altervista.org/quickbms.htm");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://aluigi.altervista.org/bms/blitz_games.bms");
        }

        private void unpackingQuickBMSPathButton_Click(object sender, EventArgs e)
        {
            var path = new OpenFileDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                unpackingQuickBMSpath.Text = path.FileName;
            }
        }

        private void unpackingScriptPathButton_Click(object sender, EventArgs e)
        {
            var path = new OpenFileDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                unpackingScriptPath.Text = path.FileName;
            }
        }

        private void unpackingInputPathButton_Click(object sender, EventArgs e)
        {
            string message = "Select file or folder? Yes - file, No - folder";
            string title = "Input path";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                var path = new OpenFileDialog();
                if (path.ShowDialog() == DialogResult.OK)
                {
                    unpackingInputPath.Text = path.FileName;
                }
            }
            else
            {
                var path = new FolderBrowserDialog();
                if (path.ShowDialog() == DialogResult.OK)
                {
                    unpackingInputPath.Text = path.SelectedPath;
                }
            }
        }

        private void unpackingOutputPathButton_Click(object sender, EventArgs e)
        {
            var path = new FolderBrowserDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                unpackingOutputPath.Text = path.SelectedPath;
            }
        }
    }
}

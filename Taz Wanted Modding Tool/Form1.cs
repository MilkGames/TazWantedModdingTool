using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Taz_Wanted_Modding_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            unpackingPlatformBox.SelectedIndex = 0;
            unpackingFileTypeBox.SelectedIndex = 0;
            decryptingPlatformBox.SelectedIndex = 0;
            decryptingFileTypeBox.SelectedIndex = 0;
        }

        private void unpackButton_Click(object sender, EventArgs e)
        {
            int    platform        = 0; // 0 - windows (default), 1 - ps2, 2 - xbox, 3 - gcn
            bool   onlyList;
            bool   separateFolder;
            bool   onlyOneArchive;
            bool   oneType;
            int    fileType        = 0; // 0 - bmp (default), 1 - gif, 2 - tga, 3 - lom, 4 - obe, 5 - ttf, 6 - wav, 7 - str+wav
            string quickBMSPath    = "\"";
            string scriptPath      = "\"";
            string inputPath       = "\"";
            string outputPath      = "\"";
            string launchOptions   = "";
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
                    case 7: fileType = 7; break; // str+wav or str+sth
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
                    case 0: inputPath += "\\*.pc\" "; break;
                    case 1: inputPath += "\\*.ps2\" "; break;
                    case 2: inputPath += "\\*.xbp\" "; break;
                    case 3: inputPath += "\\*.gcp\" "; break;
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
                    case 0: launchOptions += "\"*.bmp\" "; break;
                    case 1: launchOptions += "\"*.gif\" "; break;
                    case 2: launchOptions += "\"*.tga\" "; break;
                    case 3: launchOptions += "\"*.lom\" "; break;
                    case 4: launchOptions += "\"*.obe\" "; break;
                    case 5: launchOptions += "\"*.ttf\" "; break;
                    case 6: launchOptions += "\"*.wav\" "; break;
                    case 7:
                        if (unpackingGCNMusicCheck.Checked)
                        {
                            launchOptions += "\"*.str;*.sth\" "; break;
                        }
                        else
                        {
                            launchOptions += "\"*.str;*.wav\" "; break;
                        }
                }
            }
            
            // unpacking
            unpackCommand = " /K "+ "\"" + quickBMSPath + "-. " + launchOptions + scriptPath + inputPath + outputPath + "\"";
            // it feels like quickbms doesn't care about outputPath if I unpack only one archive
            // and i also include -. because it'll terminate the script if you unpack multiple
            // archives and choose to unpack only one type of files
            //statusLabel.Text = unpackCommand;
            Process.Start("cmd.exe", unpackCommand);
            statusLabel.Text = "Done! (or you're just waiting for it)";
            statusLabel.ForeColor = System.Drawing.Color.Green;
        }

        private static int currentPlatform = 0;

        private void decryptButton_Click(object sender, EventArgs e)
        {
            int platform = 0; // 0 - windows, 1 - ps2, 2 - xbox, 3 - gcn
            int fileType = 0; // 0 - bmp, 1 - gif, 2 - tga, 3 - obe, 4 - ttf, 5 - wav, 6 - str+wav

            // getting information
            currentPlatform = decryptingPlatformBox.SelectedIndex;
            switch (decryptingPlatformBox.SelectedIndex)
            {
                case 0: platform = 0; break; // windows
                case 1: platform = 1; break; // ps2
                case 2: platform = 2; break; // xbox
                case 3: platform = 3; break; // gcn
            }

            switch (decryptingFileTypeBox.SelectedIndex)
            {
                case 0: fileType = 0; break; // bmp
                case 1: fileType = 1; break; // gif
                case 2: fileType = 2; break; // tga
                case 3: fileType = 3; break; // obe
                case 4: fileType = 4; break; // ttf
                case 5: fileType = 5; break; // wav
                case 6: fileType = 6; break; // str+wav
            }

            // decrypting
            switch (platform)
            {
                case 0: // windows
                    switch (fileType)
                    {
                        case 0: // bmp
                            var bmpThread = new Thread(decryptBMPWindows);
                            bmpThread.Start();
                            break;
                        case 1: // gif
                            var gifThread = new Thread(decryptGIFWindows);
                            gifThread.Start();
                            break;
                        case 6: // str+wav
                            var musicThread = new Thread(decryptMusic);
                            musicThread.Start();
                            break;
                    }
                    break;
                case 1: // ps2
                    switch (fileType)
                    {
                        case 0:
                            break;
                    }
                    break;
                case 2: // xbox
                    switch (fileType)
                    {
                        case 0:
                            break;
                        case 6: // wav.str + wav
                            var musicThread = new Thread(decryptMusic);
                            musicThread.Start();
                            break;
                    }
                    break;
                case 3: // gcn
                    switch (fileType)
                    {
                        case 0:
                            break;
                        case 6: // .str + .wav or .str + .sth
                            var musicThread = new Thread(decryptMusic);
                            musicThread.Start();
                            break;
                    }
                    break;
            }
        }

        private void decryptBMPWindows()
        {
            bool                allowSubFolders;
            bool                earlyBuilds;
            bool                onlyOneFile;
            bool                replaceFiles;
            bool                outputPathExists;
            int                 headerSkip; // 20 for MS10 & earlier, 32 for MS10 & older
            IEnumerable<string> allfiles;
            string              outputPath = "";

            // getting information
            if (decryptingSearchInSubfoldersCheck.Checked) allowSubFolders = true;
            else allowSubFolders = false;

            if (decryptingEarlyBuildsCheck.Checked) earlyBuilds = true;
            else earlyBuilds = false;

            if (decryptingReplaceOriginalFilesCheck.Checked) replaceFiles = true;
            else replaceFiles = false;

            if (decryptingOnlyOneFileCheck.Checked) onlyOneFile = true;
            else onlyOneFile = false;

            if (decryptingOutputPathCheck.Checked) outputPathExists = true;
            else outputPathExists = false;

            // preparing
            string filesPath = decryptingInputPath.Text;
            if (onlyOneFile)
            {
                allfiles = new string[] {filesPath};
                // I'm not sure if it'll work as I planned, but it's much easier than creating a template
            }
            else
            {
                if (allowSubFolders) allfiles = Directory.EnumerateFiles(filesPath, "*.bmp", SearchOption.AllDirectories);
                else allfiles = Directory.EnumerateFiles(filesPath, "*.bmp", SearchOption.TopDirectoryOnly);
            }

            if (earlyBuilds) headerSkip = 20;
            else headerSkip = 32;

            if (outputPathExists) outputPath = decryptingOutputPath.Text;

            //decrypting
            foreach (String fileName in allfiles)
            {
                Invoke((MethodInvoker)delegate
                {
                    statusLabel.ForeColor = System.Drawing.Color.Black;
                    statusLabel.Text = "Processing: " + fileName;
                });

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
                    rawData[i] = gameFile[i + headerSkip];
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

                if (outputPathExists)
                {
                    string newFilePath = outputPath + "\\" + Path.GetFileName(fileName);
                    File.WriteAllBytes(newFilePath, convertedFile);
                }
                else
                {
                    if (replaceFiles) File.WriteAllBytes(fileName, convertedFile);
                    else File.WriteAllBytes(fileName + ".bmp", convertedFile);
                }
            }
            Invoke((MethodInvoker)delegate
            {
                statusLabel.Text = "Done!";
                statusLabel.ForeColor = System.Drawing.Color.Green;
            });
        }

        private void decryptGIFWindows()
        {
            bool allowSubFolders;
            bool onlyOneFile;
            bool replaceFiles;
            bool outputPathExists;
            IEnumerable<string> allfiles;
            string outputPath = "";

            // getting information
            if (decryptingSearchInSubfoldersCheck.Checked) allowSubFolders = true;
            else allowSubFolders = false;

            if (decryptingReplaceOriginalFilesCheck.Checked) replaceFiles = true;
            else replaceFiles = false;

            if (decryptingOnlyOneFileCheck.Checked) onlyOneFile = true;
            else onlyOneFile = false;

            if (decryptingOutputPathCheck.Checked) outputPathExists = true;
            else outputPathExists = false;

            // preparing
            string filesPath = decryptingInputPath.Text;
            if (onlyOneFile)
            {
                allfiles = new string[] { filesPath };
                // I'm not sure if it'll work as I planned, but it's much easier than creating a template
            }
            else
            {
                if (allowSubFolders) allfiles = Directory.EnumerateFiles(filesPath, "*.gif", SearchOption.AllDirectories);
                else allfiles = Directory.EnumerateFiles(filesPath, "*.gif", SearchOption.TopDirectoryOnly);
            }

            if (outputPathExists) outputPath = decryptingOutputPath.Text;
            foreach (String fileName in allfiles)
            {
                Invoke((MethodInvoker)delegate
                {
                    statusLabel.ForeColor = System.Drawing.Color.Black;
                    statusLabel.Text = "Processing: " + fileName;
                });

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

                    //save file
                    if (outputPathExists)
                    {
                        string newFilePath = outputPath + "\\" + Path.GetFileName(fileName);
                        newFilePath += (gifFrames - i).ToString() + ".bmp";
                        File.WriteAllBytes(newFilePath, convertedFile);
                    }
                    else
                    {
                        File.WriteAllBytes(fileName + (gifFrames - i).ToString() + ".bmp", convertedFile);
                        if (replaceFiles) File.Delete(fileName);
                    }
                }
            }
            Invoke((MethodInvoker)delegate
            {
                statusLabel.Text = "Done!";
                statusLabel.ForeColor = System.Drawing.Color.Green;
            });
        }

        private void decryptMusic()
        {
            bool allowSubFolders;
            bool earlyBuilds; // B16 for windows
            bool onlyOneFile;
            bool replaceFiles;
            bool separateChannels;
            bool outputPathExists;
            string infoFilePath;
            byte[] infoFile;
            int channels = 0;
            IEnumerable<string> allfiles;
            string vgmstreamPath = "";
            string outputPath = "";
            string decodeCommand = "";
            string txtpFilePath = "";
            string tempFilePath = "";

            // getting information
            if (decryptingSearchInSubfoldersCheck.Checked) allowSubFolders = true;
            else allowSubFolders = false;

            if (decryptingEarlyBuildsCheck.Checked) earlyBuilds = true;
            else earlyBuilds = false;

            if (decryptingReplaceOriginalFilesCheck.Checked) replaceFiles = true;
            else replaceFiles = false;

            if (decryptingOnlyOneFileCheck.Checked) onlyOneFile = true;
            else onlyOneFile = false;

            if (decryptingOutputPathCheck.Checked) outputPathExists = true;
            else outputPathExists = false;

            if (decryptingSeparateChannelsCheck.Checked) separateChannels = true;
            else separateChannels = false;

            // preparing
            string filesPath = decryptingInputPath.Text;
            if (onlyOneFile)
            {
                allfiles = new string[] { filesPath };
                // I'm not sure if it'll work as I planned, but it's much easier than creating a template
            }
            else
            {
                if (allowSubFolders) allfiles = Directory.EnumerateFiles(filesPath, "*.str", SearchOption.AllDirectories);
                else allfiles = Directory.EnumerateFiles(filesPath, "*.str", SearchOption.TopDirectoryOnly);
            }

            vgmstreamPath = decryptingVgmstreamPath.Text;
            if (outputPathExists) outputPath = decryptingOutputPath.Text;

            // decrypting
            foreach (String fileName in allfiles)
            {
                decodeCommand = "";
                Invoke((MethodInvoker)delegate
                {
                    statusLabel.ForeColor = System.Drawing.Color.Black;
                    statusLabel.Text = "Processing: " + fileName;
                });
                // searching for channels
                infoFilePath = fileName.Substring(0, fileName.Length - 4);
                switch (currentPlatform)
                {
                    case 0: // windows
                        infoFile = File.ReadAllBytes(infoFilePath);
                        channels = infoFile[216];
                        if (earlyBuilds) // B16
                        {
                            if (channels == 3) channels += 3;
                            if (channels == 1) channels++;
                        }
                        break;
                    case 1: // ps2
                        break;
                    case 2: // xbox
                        infoFile = File.ReadAllBytes(infoFilePath);
                        channels = infoFile[80];
                        if (channels == 3) channels += 3;
                        if (channels == 1) channels++;
                        break;
                    case 3: // gcn
                        if (decryptingGCNMusicCheck.Checked) infoFilePath += ".sth"; // ntsc
                        infoFile = File.ReadAllBytes(infoFilePath);
                        channels = infoFile[83];
                        if (channels == 3) channels += 3;
                        if (channels == 1) channels++;
                        break;
                }

                // decode music

                if (!separateChannels || channels == 1 || channels == 2)
                {
                    // you don't need to create .txtp in this case
                    if (outputPathExists)
                    {
                        decodeCommand += "-o " + "\"" + outputPath + "\\" + Path.GetFileName(infoFilePath) + "\"";
                        decodeCommand += " -i " + "\"" + fileName + "\"";
                        ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                        vgmstream.Arguments = decodeCommand;
                        vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                        var vgmstreamProc = Process.Start(vgmstream);
                        vgmstreamProc.WaitForExit();
                    }
                    else
                    {
                        if (replaceFiles)
                        {
                            if (currentPlatform == 3)
                            {
                                if (decryptingGCNMusicCheck.Checked)
                                {
                                    tempFilePath = infoFilePath;
                                    tempFilePath = fileName.Substring(0, fileName.Length - 4);
                                    decodeCommand += "-o " + "\"" + tempFilePath + "\"";
                                }
                                else decodeCommand += "-o " + "\"" + infoFilePath + "\"";
                                decodeCommand += " -i " + "\"" + fileName + "\"";
                                ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                                vgmstream.Arguments = decodeCommand;
                                vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                                var vgmstreamProc = Process.Start(vgmstream);
                                vgmstreamProc.WaitForExit();
                                if (decryptingGCNMusicCheck.Checked)
                                {
                                    File.Delete(fileName);
                                    File.Delete(infoFilePath);
                                }
                                File.Delete(fileName);
                            }
                            else
                            {
                                decodeCommand += "-o " + "\"" + infoFilePath + "\"" + " -i " + "\"" + fileName + "\"";
                                ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                                vgmstream.Arguments = decodeCommand;
                                vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                                var vgmstreamProc = Process.Start(vgmstream);
                                vgmstreamProc.WaitForExit();
                                File.Delete(fileName);
                            }
                        }
                        else
                        {
                            decodeCommand += "-i " + "\"" + fileName + "\"";
                            ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                            vgmstream.Arguments = decodeCommand;
                            vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                            var vgmstreamProc = Process.Start(vgmstream);
                            vgmstreamProc.WaitForExit();
                        }
                    }
                }
                else
                {
                    if (channels == 3)
                    {
                        for (int i = 3; i > 0; i--) // vgmstream bug - don't touch!
                        {
                            decodeCommand = "";
                            string channelNumber = "";
                            string songNumber = "";
                            switch (i)
                            {
                                case 1:
                                    channelNumber = "1";
                                    songNumber = "normal loop.wav";
                                    break;
                                case 2:
                                    channelNumber = "2";
                                    songNumber = "tiptoe loop.wav";
                                    break;
                                case 3:
                                    channelNumber = "3";
                                    songNumber = "spin loop.wav";
                                    break;
                            }
                            txtpFilePath = fileName + ".txtp";
                            using (FileStream fs = File.Create(txtpFilePath))
                            {
                                byte[] info = new UTF8Encoding(true).GetBytes(fileName + " #C" + channelNumber);
                                fs.Write(info, 0, info.Length);
                            }

                            if (outputPathExists)
                            {
                                tempFilePath = fileName;
                                tempFilePath = tempFilePath.Substring(0, tempFilePath.Length - 19);
                                tempFilePath += songNumber;
                                decodeCommand += "-o " + "\"" + outputPath + "\\" + Path.GetFileName(tempFilePath) + "\"";
                                decodeCommand += " -i " + "\"" + txtpFilePath + "\"";
                                ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                                vgmstream.Arguments = decodeCommand;
                                vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                                var vgmstreamProc = Process.Start(vgmstream);
                                vgmstreamProc.WaitForExit();
                            }
                            else
                            {
                                tempFilePath = fileName;
                                tempFilePath = tempFilePath.Substring(0, tempFilePath.Length - 19);
                                tempFilePath += songNumber;
                                decodeCommand += "-o " + "\"" + tempFilePath + "\"";
                                decodeCommand += " -i " + "\"" + txtpFilePath + "\"";
                                ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                                vgmstream.Arguments = decodeCommand;
                                vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                                var vgmstreamProc = Process.Start(vgmstream);
                                vgmstreamProc.WaitForExit();
                            }
                        }
                        if (replaceFiles)
                        {
                            File.Delete(fileName);
                        }
                        File.Delete(txtpFilePath);
                    }
                    else
                    {
                        for (int i = 3; i > 0; i--) // vgmstream bug - don't touch!
                        {
                            decodeCommand = "";
                            string channelNumber = "";
                            string songNumber = "";
                            switch (i)
                            {
                                case 1:
                                    channelNumber = "1,2";
                                    songNumber = "normal loop.wav";
                                    break;
                                case 2:
                                    channelNumber = "3,4";
                                    songNumber = "tiptoe loop.wav";
                                    break;
                                case 3:
                                    channelNumber = "5,6";
                                    songNumber = "spin loop.wav";
                                    break;
                            }
                            txtpFilePath = fileName + ".txtp";
                            using (FileStream fs = File.Create(txtpFilePath))
                            {
                                byte[] info = new UTF8Encoding(true).GetBytes(Path.GetFileName(fileName) + " #C" + channelNumber);
                                fs.Write(info, 0, info.Length);
                            }

                            if (outputPathExists)
                            {
                                tempFilePath = fileName;
                                tempFilePath = tempFilePath.Substring(0, tempFilePath.Length - 19);
                                tempFilePath += songNumber;
                                decodeCommand += "-o " + "\"" + outputPath + "\\" + Path.GetFileName(tempFilePath) + "\"";
                                decodeCommand += " -i " + "\"" + txtpFilePath + "\"";
                                ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                                vgmstream.Arguments = decodeCommand;
                                vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                                var vgmstreamProc = Process.Start(vgmstream);
                                vgmstreamProc.WaitForExit();
                            }
                            else
                            {
                                tempFilePath = fileName;
                                tempFilePath = tempFilePath.Substring(0, tempFilePath.Length - 19);
                                tempFilePath += songNumber;
                                decodeCommand += "-o \"" + tempFilePath + "\"";
                                decodeCommand += " -i " + "\"" + txtpFilePath + "\"";
                                ProcessStartInfo vgmstream = new ProcessStartInfo(vgmstreamPath);
                                vgmstream.Arguments = decodeCommand;
                                vgmstream.WindowStyle = ProcessWindowStyle.Hidden;
                                var vgmstreamProc = Process.Start(vgmstream);
                                vgmstreamProc.WaitForExit();
                            }
                        }
                        if (replaceFiles)
                        {
                            File.Delete(infoFilePath);
                            File.Delete(fileName);
                        }
                        File.Delete(txtpFilePath);
                    }
                }
            }
            Invoke((MethodInvoker)delegate
            {
                statusLabel.Text = "Done!";
                statusLabel.ForeColor = System.Drawing.Color.Green;
            });
        }
       
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

        private void vgmstreamDownloadLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/vgmstream/vgmstream-releases/releases/download/nightly/vgmstream-win64.zip");
        }

        private void unpackingPlatformBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            unpackingFileTypeBox.SelectedIndex = 0;
        }

        private void unpackingGCNMusicCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (unpackingGCNMusicCheck.Checked)
            {
                if (unpackingFileTypeBox.SelectedIndex == 7)
                {
                    unpackingFileTypeBox.Enabled = false;
                    unpackingFileTypeBox.SelectedIndex = 0;
                    unpackingFileTypeBox.Items[7] = ".str + .sth (music)";
                    unpackingFileTypeBox.Enabled = false;
                    unpackingFileTypeBox.SelectedIndex = 7;
                }
                else unpackingFileTypeBox.Items[7] = ".str + .sth (music)";
            }
            else
            {
                if (unpackingFileTypeBox.SelectedIndex == 7)
                {
                    unpackingFileTypeBox.Enabled = false;
                    unpackingFileTypeBox.SelectedIndex = 0;
                    unpackingFileTypeBox.Items[7] = ".str + .wav (music)";
                    unpackingFileTypeBox.Enabled = false;
                    unpackingFileTypeBox.SelectedIndex = 7;
                }
                else unpackingFileTypeBox.Items[7] = ".str + .wav (music)";
            }
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

        private void unpackingOneTypeCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (unpackingOneTypeCheck.Checked)
            {
                unpackingFileTypeBox.Enabled = true;
            }
            else unpackingFileTypeBox.Enabled = false;
        }

        private void unpackingFileTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (unpackingFileTypeBox.Enabled == false &&
                unpackingOneTypeCheck.Checked)
            {
                unpackingFileTypeBox.Enabled = true;
                return;
            }
            if (unpackingPlatformBox.SelectedIndex == 3 &&
                unpackingFileTypeBox.SelectedIndex == 7)
            {
                unpackingGCNMusicCheck.Visible = true;
                unpackingGCNMusicCheck.Enabled = true;
            }
            else
            {
                unpackingGCNMusicCheck.Enabled = false;
                unpackingGCNMusicCheck.Visible = false;
                unpackingGCNMusicCheck.Checked = false;
            }
        }

        private void unpackingOnlyListCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!unpackingOnlyListCheck.Enabled) return;
            if (unpackingOnlyListCheck.Checked)
            {
                unpackingSeparateFolderCheck.Enabled = false;
                unpackingSeparateFolderCheck.Checked = false;
                unpackingOnlyOneArchiveCheck.Enabled = false;
                unpackingOnlyOneArchiveCheck.Checked = false;
            }
            else
            {
                unpackingSeparateFolderCheck.Enabled = true;
                unpackingOnlyOneArchiveCheck.Enabled = true;
            }
        }

        private void unpackingSeparateFolderCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!unpackingSeparateFolderCheck.Enabled) return;
            if (unpackingSeparateFolderCheck.Checked)
            {
                unpackingOnlyOneArchiveCheck.Enabled = false;
                unpackingOnlyOneArchiveCheck.Checked = false;
                unpackingOnlyListCheck.Enabled = false;
                unpackingOnlyListCheck.Checked = false;
            }
            else
            {
                unpackingOnlyOneArchiveCheck.Enabled = true;
                unpackingOnlyListCheck.Enabled = true;
            }
        }

        private void unpackingOnlyOneArchiveCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!unpackingOnlyOneArchiveCheck.Enabled) return;
            if (unpackingOnlyOneArchiveCheck.Checked)
            {
                unpackingSeparateFolderCheck.Enabled = false;
                unpackingSeparateFolderCheck.Checked = false;
                unpackingOnlyListCheck.Enabled = false;
                unpackingOnlyListCheck.Checked = false;
            }
            else
            {
                unpackingSeparateFolderCheck.Enabled = true;
                unpackingOnlyListCheck.Enabled = true;
            }
        }

        private void decryptingInputPathButton_Click(object sender, EventArgs e)
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
                    decryptingInputPath.Text = path.FileName;
                }
            }
            else
            {
                var path = new FolderBrowserDialog();
                if (path.ShowDialog() == DialogResult.OK)
                {
                    decryptingInputPath.Text = path.SelectedPath;
                }
            }
        }

        private void decryptingOutputPathButton_Click(object sender, EventArgs e)
        {
            var path = new FolderBrowserDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                decryptingOutputPath.Text = path.SelectedPath;
            }
        }

        private void decryptingVgmstreamPathButton_Click(object sender, EventArgs e)
        {
            var path = new OpenFileDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                decryptingVgmstreamPath.Text = path.FileName;
            }
        }

        private void decryptingPlatformBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            unpackingFileTypeBox.SelectedIndex = 0;
            decryptingFileTypeBox_SelectedIndexChanged(sender, e);
        }

        private void decryptingFileTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (decryptingFileTypeBox.Enabled == false)
            {
                decryptingFileTypeBox.Enabled = true;
                return;
            }
            if (decryptingPlatformBox.SelectedIndex == 3 &&
                decryptingFileTypeBox.SelectedIndex == 6)
            {
                decryptingGCNMusicCheck.Visible = true;
                decryptingGCNMusicCheck.Enabled = true;
            }
            else
            {
                decryptingGCNMusicCheck.Enabled = false;
                decryptingGCNMusicCheck.Visible = false;
                decryptingGCNMusicCheck.Checked = false;
            }
            decryptingSeparateChannelsCheck.Visible = false;
            decryptingVgmstreamPathButton.Enabled = false;
            decryptingVgmstreamPath.Enabled = false;
            decryptingEarlyBuildsCheck.Visible = false;
            decryptingEarlyBuildsCheck2.Visible = false;
            decryptButton.Enabled = false;
            switch (decryptingPlatformBox.SelectedIndex)
            {
                case 0: // windows
                    switch (decryptingFileTypeBox.SelectedIndex)
                    {
                        case 0: // bmp
                            decryptingEarlyBuildsCheck.Text = "MileStone #10 or earlier";
                            decryptingEarlyBuildsCheck.Visible = true;
                            decryptButton.Enabled = true;
                            break;
                        case 1: // gif
                            decryptButton.Enabled = true;
                            break;
                        case 6: // .str + .wav
                            decryptingEarlyBuildsCheck.Text = "Beta #16";
                            decryptingEarlyBuildsCheck.Visible = true;
                            decryptingEarlyBuildsCheck2.Visible = true;
                            decryptingSeparateChannelsCheck.Visible = true;
                            decryptingVgmstreamPathButton.Enabled = true;
                            decryptingVgmstreamPath.Enabled = true;
                            decryptButton.Enabled = true;
                            break;
                    }
                    break;
                case 1: // ps2
                    switch (decryptingFileTypeBox.SelectedIndex)
                    {
                        case 6: // .str + .wav
                            break;
                    }
                    break;
                case 2: // xbox
                    switch (decryptingFileTypeBox.SelectedIndex)
                    {
                        case 6: // .str + .wav
                            decryptingSeparateChannelsCheck.Visible = true;
                            decryptingVgmstreamPathButton.Enabled = true;
                            decryptingVgmstreamPath.Enabled = true;
                            decryptButton.Enabled = true;
                            break;
                    }
                    break;
                case 3: // gcn
                    switch (decryptingFileTypeBox.SelectedIndex)
                    {
                        case 6: // .str + .sth or .str + .wav
                            decryptingSeparateChannelsCheck.Visible = true;
                            decryptingVgmstreamPathButton.Enabled = true;
                            decryptingVgmstreamPath.Enabled = true;
                            decryptingGCNMusicCheck.Visible = true;
                            decryptingGCNMusicCheck.Enabled = true;
                            decryptButton.Enabled = true;
                            break;
                    }
                    break;
            }
        }

        private void decryptingSearchInSubfoldersCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!decryptingSearchInSubfoldersCheck.Enabled) return;
            if (decryptingSearchInSubfoldersCheck.Checked)
            {
                decryptingOnlyOneFileCheck.Enabled = false;
                decryptingOnlyOneFileCheck.Checked = false;
            }
            else
            {
                decryptingOnlyOneFileCheck.Enabled = true;
            }
        }

        private void decryptingOnlyOneFileCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!decryptingOnlyOneFileCheck.Enabled) return;
            if (decryptingOnlyOneFileCheck.Checked)
            {
                decryptingSearchInSubfoldersCheck.Enabled = false;
                decryptingSearchInSubfoldersCheck.Checked = false;
            }
            else
            {
                decryptingSearchInSubfoldersCheck.Enabled = true;
            }
        }

        private void decryptingOutputPathCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!decryptingOutputPathCheck.Enabled) return;
            if (decryptingOutputPathCheck.Checked)
            {
                decryptingReplaceOriginalFilesCheck.Enabled = false;
                decryptingReplaceOriginalFilesCheck.Checked = false;
                decryptingOutputPath.Enabled = true;
            }
            else
            {
                decryptingReplaceOriginalFilesCheck.Enabled = true;
                decryptingOutputPath.Enabled = false;
            }
        }

        private void decryptingReplaceOriginalFilesCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!decryptingReplaceOriginalFilesCheck.Enabled) return;
            if (decryptingReplaceOriginalFilesCheck.Checked)
            {
                decryptingOutputPathCheck.Enabled = false;
                decryptingOutputPathCheck.Checked = false;
            }
            else
            {
                decryptingOutputPathCheck.Enabled = true;
            }
        }

        private void decryptingGCNMusicCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (decryptingGCNMusicCheck.Checked)
            {
                if (decryptingFileTypeBox.SelectedIndex == 6)
                {
                    decryptingFileTypeBox.Enabled = false;
                    decryptingFileTypeBox.SelectedIndex = 0;
                    decryptingFileTypeBox.Items[6] = ".str + .sth (music)";
                    decryptingFileTypeBox.Enabled = false;
                    decryptingFileTypeBox.SelectedIndex = 6;
                }
                else decryptingFileTypeBox.Items[6] = ".str + .sth (music)";
            }
            else
            {
                if (decryptingFileTypeBox.SelectedIndex == 6)
                {
                    decryptingFileTypeBox.Enabled = false;
                    decryptingFileTypeBox.SelectedIndex = 0;
                    decryptingFileTypeBox.Items[6] = ".str + .wav (music)";
                    decryptingFileTypeBox.Enabled = false;
                    decryptingFileTypeBox.SelectedIndex = 6;
                }
                else decryptingFileTypeBox.Items[6] = ".str + .wav (music)";
            }
        }
    }
}

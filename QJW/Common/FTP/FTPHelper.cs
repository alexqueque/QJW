using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QJW.Common
{
    /// <summary>
    /// ftp�ļ��ϴ������ز�����
    /// </summary>
    public class FTPHelper
    {

        /// <summary>
        /// ftp�û���������Ϊ����
        /// </summary>
        private string ftpUser;

        /// <summary>
        /// ftp�û����룬����Ϊ����
        /// </summary>
        private string ftpPassWord;

        /// <summary>
        ///ͨ���û������������ӵ�FTP������
        /// </summary>
        /// <param name="ftpUser">ftp�û���������Ϊ����</param>
        /// <param name="ftpPassWord">ftp��½���룬����Ϊ����</param>
        public FTPHelper(string ftpUser, string ftpPassWord)
        {
            this.ftpUser = ftpUser;
            this.ftpPassWord = ftpPassWord;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public FTPHelper()
        {
            this.ftpUser = "";
            this.ftpPassWord = "";
        }

        /// <summary>
        /// �ϴ��ļ���Ftp������
        /// </summary>
        /// <param name="UpLoadUri">���ϴ����ļ�����Ϊftp�������ļ���uri,��"ftp://192.168.1.104/capture-212.avi"</param>
        /// <param name="upLoadFile">Ҫ�ϴ��ı��ص��ļ�·������D:\capture-2.avi</param>
        public void UpLoadFile(string UpLoadUri, string upLoadFile)
        {
            Stream requestStream = null;
            FileStream fileStream = null;
            FtpWebResponse uploadResponse = null;

            try
            {
                Uri uri = new Uri(UpLoadUri);

                FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(uri);
                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;

                uploadRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);

                requestStream = uploadRequest.GetRequestStream();
                fileStream = File.Open(upLoadFile, FileMode.Open);

                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    requestStream.Write(buffer, 0, bytesRead);
                }

                requestStream.Close();

                uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();

            }
            catch (Exception ex)
            {
                throw new Exception("�ϴ��ļ���ftp�����������ļ�����" + upLoadFile + "�쳣��Ϣ��" + ex.ToString());
            }
            finally
            {
                if (uploadResponse != null)
                    uploadResponse.Close();
                if (fileStream != null)
                    fileStream.Close();
                if (requestStream != null)
                    requestStream.Close();
            }
        }

        /// <summary>
        /// ��ftp�����ļ������ط�����
        /// </summary>
        /// <param name="downloadUrl">Ҫ���ص�ftp�ļ�·������ftp://192.168.1.104/capture-2.avi</param>
        /// <param name="saveFileUrl">���ر����ļ���·������(@"d:\capture-22.avi"</param>
        public void DownLoadFile(string downloadUrl, string saveFileUrl)
        {
            Stream responseStream = null;
            FileStream fileStream = null;
            StreamReader reader = null;

            try
            {
                if (!File.Exists(saveFileUrl))
                {
                    // string downloadUrl = "ftp://192.168.1.104/capture-2.avi";

                    FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(downloadUrl);
                    downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                    //string ftpUser = "yoyo";
                    //string ftpPassWord = "123456";
                    downloadRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);

                    FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                    responseStream = downloadResponse.GetResponseStream();

                    fileStream = File.Create(saveFileUrl);
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while (true)
                    {
                        bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                            break;
                        fileStream.Write(buffer, 0, bytesRead);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("��ftp�����������ļ������ļ�����" + downloadUrl + "�쳣��Ϣ��" + ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }


        /// <summary>
        /// ��FTP�����ļ������ط�����,֧�ֶϵ�����
        /// </summary>
        /// <param name="ftpUri">ftp�ļ�·������"ftp://localhost/test.txt"</param>
        /// <param name="saveFile">�����ļ���·������C:\\test.txt</param>
        public void BreakPointDownLoadFile(string ftpUri, string saveFile)
        {
            System.IO.FileStream fs = null;
            System.Net.FtpWebResponse ftpRes = null;
            System.IO.Stream resStrm = null;
            try
            {
                //�����ļ���URI
                Uri u = new Uri(ftpUri);
                //�趨�����ļ��ı���·��
                string downFile = saveFile;

                //FtpWebRequest������
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //�趨�û���������
                ftpReq.Credentials = new System.Net.NetworkCredential(ftpUser, ftpPassWord);
                //Method��WebRequestMethods.Ftp.DownloadFile("RETR")�趨
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //Ҫ�����˺�ر�����
                ftpReq.KeepAlive = false;
                //ʹ��ASCII��ʽ����
                ftpReq.UseBinary = false;
                //�趨PASSIVE��ʽ��Ч
                ftpReq.UsePassive = false;

                //�ж��Ƿ��������
                //����д�������ļ���FileStream

                if (System.IO.File.Exists(downFile))
                {
                    //��������
                    ftpReq.ContentOffset = (new System.IO.FileInfo(downFile)).Length;
                    fs = new System.IO.FileStream(
                       downFile, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
                else
                {
                    //һ������
                    fs = new System.IO.FileStream(
                        downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                }

                //ȡ��FtpWebResponse
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //Ϊ�������ļ�ȡ��Stream
                resStrm = ftpRes.GetResponseStream();
                //д�����ص�����
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int readSize = resStrm.Read(buffer, 0, buffer.Length);
                    if (readSize == 0)
                        break;
                    fs.Write(buffer, 0, readSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("��ftp�����������ļ������ļ�����" + ftpUri + "�쳣��Ϣ��" + ex.ToString());
            }
            finally
            {
                fs.Close();
                resStrm.Close();
                ftpRes.Close();
            }
        }

        #region ��FTP�����������ļ��У������ļ����µ��ļ����ļ���

        /// <summary>
        /// �г�FTP���������浱ǰĿ¼�������ļ���Ŀ¼
        /// </summary>
        /// <param name="ftpUri">FTPĿ¼</param>
        /// <returns></returns>
        public List<FileStruct> ListFilesAndDirectories(string ftpUri)
        {
            WebResponse webresp = null;
            StreamReader ftpFileListReader = null;
            FtpWebRequest ftpRequest = null;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpUri));
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                ftpRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);
                webresp = ftpRequest.GetResponse();
                ftpFileListReader = new StreamReader(webresp.GetResponseStream(), Encoding.Default);
            }
            catch (Exception ex)
            {
                throw new Exception("��ȡ�ļ��б����������Ϣ���£�" + ex.ToString());
            }
            string Datastring = ftpFileListReader.ReadToEnd();
            return GetList(Datastring);

        }

        /// <summary>
        /// �г�FTPĿ¼�µ������ļ�
        /// </summary>
        /// <param name="ftpUri">FTPĿ¼</param>
        /// <returns></returns>
        public List<FileStruct> ListFiles(string ftpUri)
        {
            List<FileStruct> listAll = ListFilesAndDirectories(ftpUri);
            List<FileStruct> listFile = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (!file.IsDirectory)
                {
                    listFile.Add(file);
                }
            }
            return listFile;
        }


        /// <summary>
        /// �г�FTPĿ¼�µ�����Ŀ¼
        /// </summary>
        /// <param name="ftpUri">FRTPĿ¼</param>
        /// <returns>Ŀ¼�б�</returns>
        public List<FileStruct> ListDirectories(string ftpUri)
        {
            List<FileStruct> listAll = ListFilesAndDirectories(ftpUri);
            List<FileStruct> listDirectory = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (file.IsDirectory)
                {
                    listDirectory.Add(file);
                }
            }
            return listDirectory;
        }

        /// <summary>
        /// ����ļ���Ŀ¼�б�
        /// </summary>
        /// <param name="datastring">FTP���ص��б��ַ���Ϣ</param>
        private List<FileStruct> GetList(string datastring)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            string[] dataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (string s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (!(f.Name == "." || f.Name == ".."))
                    {
                        myListArray.Add(f);
                    }
                }
            }
            return myListArray;
        }
        /// <summary>
        /// ��Unix��ʽ�з����ļ���Ϣ
        /// </summary>
        /// <param name="Record">�ļ���Ϣ</param>
        private FileStruct ParseFileStructFromUnixStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            f.Flags = processstr.Substring(0, 10);
            f.IsDirectory = (f.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //����һ����
            f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Group = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //����һ����
            string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            if (yearOrTime.IndexOf(":") >= 0)  //time
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }
            f.CreateTime = DateTime.Parse(_cutSubstringFromStringWithTrim(ref processstr, ' ', 8));
            f.Name = processstr;   //����������
            return f;
        }

        /// <summary>
        /// ��Windows��ʽ�з����ļ���Ϣ
        /// </summary>
        /// <param name="Record">�ļ���Ϣ</param>
        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, 2);// StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            return f;
        }
        /// <summary>
        /// ����һ���Ĺ�������ַ�����ȡ
        /// </summary>
        /// <param name="s">��ȡ���ַ���</param>
        /// <param name="c">���ҵ��ַ�</param>
        /// <param name="startIndex">���ҵ�λ��</param>
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }
        /// <summary>
        /// �ж��ļ��б�ķ�ʽWindow��ʽ����Unix��ʽ
        /// </summary>
        /// <param name="recordList">�ļ���Ϣ�б�</param>
        private FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                 && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8
                 && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }

        /// <summary>  
        /// ��FTP���������ļ���  
        /// </summary>  
        /// <param name="ftpDir">FTP�ļ���·��</param>  
        /// <param name="saveDir">����ı����ļ���·��</param>  
        public void DownFtpDir(string ftpDir, string saveDir)
        {
            List<FileStruct> files = ListFilesAndDirectories(ftpDir);
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            foreach (FileStruct f in files)
            {
                if (f.IsDirectory) //�ļ��У��ݹ��ѯ
                {
                    DownFtpDir(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                }
                else //�ļ���ֱ������
                {
                    DownLoadFile(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                }
            }
        }


        #endregion
    }

    #region �ļ���Ϣ�ṹ
    public struct FileStruct
    {
        public string Flags;
        public string Owner;
        public string Group;
        public bool IsDirectory;
        public DateTime CreateTime;
        public string Name;
    }
    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }
    #endregion
}
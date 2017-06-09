using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MoveComicFiles
{
    class Program
    {

        static void Main(string[] args)
        {
			string _Source = "/Volumes/My Passport/Comics/Current Week/DC";
			string _Destination = "/Volumes/My Passport/Comics/DC";

			Console.WriteLine("****************************************");
			Console.WriteLine("Source Directory => " + _Source);
			Console.WriteLine("Desination => " + _Destination);

			if (Directory.Exists(_Source))
			{
				foreach (var _file in Directory.GetFiles(_Source))
				{
					if (Path.GetExtension(_file).Contains("cbr") || Path.GetExtension(_file).Contains("cbz"))
					{
	
                        var _FileNameOnly = Path.GetFileName(_file);

                        if(!_FileNameOnly.Substring(0,1).Equals(("."))){
							Console.WriteLine(_FileNameOnly);
                            var _RightPart = _FileNameOnly.Substring(_FileNameOnly.IndexOf("("));
                            var _LeftPart = _FileNameOnly.Replace(_RightPart, string.Empty);
                            var _FinaleString = Regex.Replace(_LeftPart, @"[\d]", string.Empty).Trim();

                            Console.WriteLine("FinalString = " + _FinaleString);

                            bool _bFound = false;
                            string _DirFoundIn = String.Empty;
                            foreach (var _dir in Directory.GetDirectories(_Destination))
                            {
                                if (Path.GetFileName(_dir).Equals(_FinaleString)){
                                    _bFound = true;
                                    _DirFoundIn = _dir;
                                    break;
                                }
                                else{
                                    _bFound = false;
                                    _DirFoundIn = string.Empty;
                                }
                            }
                            if(_bFound){
                                Console.WriteLine("directory found, copy file");
                                var _ToDir = Path.Combine(_Destination, _FinaleString, _FileNameOnly);
                                if(!File.Exists(_ToDir))
                                    File.Copy(_file, _ToDir);
                                else
                                    Console.WriteLine("file exist already");
                            }
                            else{
                                Console.WriteLine("directory not found, create it, then copy it");
                                var _ToDir = Path.Combine(_Destination, _FinaleString);
                                Directory.CreateDirectory(_ToDir);
                                _ToDir = Path.Combine(_ToDir, _FinaleString);

                                if (!File.Exists(_ToDir))
                                    File.Copy(_file, _ToDir);
                                else
                                    Console.WriteLine("file exist already");
                            }
                        }
					}
				}

                Console.WriteLine("Verify and Delete files if all is ok");
                foreach (var _file in Directory.GetFiles(_Source)){
                    if (Path.GetExtension(_file).Contains("cbr") || Path.GetExtension(_file).Contains("cbz"))
                    {
                        var _FileNameOnly = Path.GetFileName(_file);
                        if(FileExistInDestination(_FileNameOnly, _Destination)){
                            Console.WriteLine("[" + _FileNameOnly + "] ok to delete file");
                            File.Delete(_file);
                        }
                    }
                }
			}
        }

        static public bool FileExistInDestination(string _FileName, string _PathToSearch){
            var allFiles = Directory.GetFiles(_PathToSearch, "*.*", SearchOption.AllDirectories);

            foreach (var _file in allFiles){
                if (_file.Contains(_FileName)){
                    return true;
                }
            }

            return false;
        }
    }
}

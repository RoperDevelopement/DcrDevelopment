using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Edocs.Convert.Viedos.Images.PropConst;
using Edocs.Convert.Viedos.Images.Controls;
using System.Windows.Forms;
using Edocs.HelperUtilities;
using System.IO;
using System.Drawing;
using static System.Net.WebRequestMethods;
using Edocs.Convert.Viedos.Images.Converters;
using Edocs.Convert.Viedos.Arguments;
using Edocs.Convert.Viedos.Enums;
using Edocs.Convert.Viedos.Images.Helpers;
using FFMpegSharp.FFMPEG.Exceptions;
using Newtonsoft.Json.Linq;

namespace Edocs.Convert.Viedos.Images.Converters
{
    public sealed class ConvertImagesToViedo
    {
        ConvertImagesToViedo() { }
        private static readonly object lockCheck = new object();
        private static ConvertImagesToViedo instance = null;
        public static ConvertImagesToViedo ConvertImagesToViedoInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockCheck)
                    {
                        if (instance == null)
                        {
                            instance = new ConvertImagesToViedo();
                        }
                    }
                }
                return instance;
            }
        }

        public async Task SaveImageAsPing(IDictionary<int, string> dicLoc)
        {
            Utilities.DeleteFiles(ConstProp.ImageFolder);
            int width = ConstProp.ImageSizeViedoWidth;
            int height = ConstProp.ImageSizeViedoHeight;
            foreach (KeyValuePair<int, string> png in dicLoc)
            {
                byte[] imgByte = System.IO.File.ReadAllBytes(png.Value);
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(png.Value))
                {
                    System.Drawing.Image newImg = ConstProp.ResizeImage(img, new Size(width, width));
                    string sFileName = $"{Path.Combine(ConstProp.ImageFolder, $"{Path.GetFileNameWithoutExtension(png.Value)}.png")}";
                    newImg.Save(sFileName, System.Drawing.Imaging.ImageFormat.Png);
                   

                }
                
            }
            ConvertViedoToImage.ConvertViedoToImageInstance.CopyImagesToWF().ConfigureAwait(false).GetAwaiter().GetResult();

        }
        public async Task CreateViedo(string viedoFolder)
        {
            VideoLibrary.VeidoJoinFolder = viedoFolder; 
            Utilities.DeleteFiles(ConstProp.ViedoCreateFolder);
        var imageSet = new List<ImageInfo>();
        Directory.EnumerateFiles(VideoLibrary.ImageDirectory.FullName)
                    .Where(file => file.ToLower().EndsWith(".png"))
                    .ToList()
                    .ForEach(file =>
                    {
            for (int i = 0; i < 15; i++)
{
                imageSet.Add(new ImageInfo(file));
            }
        });
            var result = JoinImageSequence(VideoLibrary.ImageJoinOutput, images: imageSet.ToArray());
            Console.WriteLine();
        }

        /// <summary>
        /// Converts an image sequence to a video.
        /// </summary>
        /// <param name="output">Output video file.</param>
        /// <param name="frameRate">FPS</param>
        /// <param name="images">Image sequence collection</param>
        /// <returns>Output video information.</returns>
      //  public VideoInfo JoinImageSequence(FileInfo output, double frameRate = 30, params ImageInfo[] images)
        public VideoInfo JoinImageSequence(FileInfo output, double frameRate = 30, params ImageInfo[] images)
        {
            //var temporaryImageFiles = images.Select((image, index) =>
            //{
            //    FFMpegHelper.ConversionSizeExceptionCheck(Image.FromFile(image.FullName));
            //    var destinationPath = image.FullName.Replace(image.Name, $"{index.ToString().PadLeft(9, '0')}{image.Extension}");
            //    Utilities.CopyFile(image.FullName, destinationPath,true);

            //    return destinationPath;
            //}).ToList();
            int index = 1;
            foreach(ImageInfo info in images)
            {
                     var destinationPath = info.FullName.Replace(info.Name, $"{index.ToString().PadLeft(9, '0')}{info.Extension}");
                    Utilities.CopyFile(info.FullName, destinationPath,true);
                index++;
            }
            var firstImage = images.First();

            var args = ArgumentsStringifier.FrameRate(frameRate) +
                ArgumentsStringifier.Size(new Size(firstImage.Width, firstImage.Height)) +
                ArgumentsStringifier.StartNumber(0) +
                ArgumentsStringifier.Input($"{firstImage.Directory}\\%09d.png") +
                ArgumentsStringifier.FrameOutputCount(images.Length) +
                ArgumentsStringifier.Video(VideoCodec.LibX264) +
                ArgumentsStringifier.Output(output);

            //try
            //{
            //    if (!RunProcess(args, output))
            //    {
            //        throw new FFMpegException(FFMpegExceptionType.Operation, "Could not join the provided image sequence.");
            //    }

            //    return new VideoInfo(output);
            //}
            //finally
            //{
            //    Cleanup(temporaryImageFiles);
            //}
            Utilities.RunTask(ConstProp.FFmpegExe, args, true, true, false, System.Diagnostics.ProcessWindowStyle.Minimized, string.Empty, string.Empty);
            //return new VideoInfo(output);
            return null;
        }
    }
}

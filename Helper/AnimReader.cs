using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Helper
{
    public class AnimReader
    {
        public string FileName { get; set; }

        public AnimReader (string filename)
        {
            FileName = filename;
        }

        public PopAnim ReadPam()
        {
            PopAnim anim = new PopAnim();
            using (BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                if (File.Exists(FileName))
                {
                    //Animation properties
                    anim.Anim = Path.GetFileNameWithoutExtension(FileName);
                    //Sprite collection
                    anim.Sprites = new List<Sprite>();
                    //Sub Anim collection
                    anim.SubAnims = new List<SubAnim>();
                    //Header: 17 Bytes
                    byte[] header = reader.ReadBytes(17);
                    //Total sprites - Int16
                    short spriteCount = reader.ReadInt16();
                    //Reading sprites
                    for (short i = 0; i < spriteCount; i++)
                    {
                        //Name's length - Int16
                        short nameLength = reader.ReadInt16();
                        //Sprite name | Sprite ID - String
                        string spriteName = BinaryConverter.ByteToString(reader.ReadBytes(nameLength));
                        //Width - Int16
                        short width = reader.ReadInt16();
                        //Height - Int16
                        short height = reader.ReadInt16();
                        //Linear transformations - Int32 (divided by 65536)
                        double x1 = (double)reader.ReadInt32() / 65536;
                        double y1 = (double)reader.ReadInt32() / 65536;
                        double x2 = (double)reader.ReadInt32() / 65536;
                        double y2 = (double)reader.ReadInt32() / 65536;
                        //Left - Int16
                        short left = reader.ReadInt16();
                        //Top - Int16
                        short top = reader.ReadInt16();
                        //Split name
                        string[] name = spriteName.Split('|');
                        //Sprite properties
                        SpriteProperties properties = new SpriteProperties
                        {
                            ID = name.Length > 1 ? name[1] : string.Empty,
                            Width = width,
                            Height = height,
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Left = left,
                            Top = top
                        };
                        //Sprite item
                        Sprite sprite = new Sprite
                        {
                            ID = name[0],
                            Properties = properties
                        };
                        //Add to collection
                        anim.Sprites.Add(sprite);
                    }
                    //Total Sub Anims - Int16
                    short subAnimCount = reader.ReadInt16();
                    //Transform Array (2 Dimension)
                    SubAnimTransform[,] sats = new SubAnimTransform[subAnimCount, 255];
                    //Reading Sub Anims
                    for (short i = 0; i < subAnimCount; i++)
                    {
                        //Name's length - Int16
                        short nameLength = reader.ReadInt16();
                        //Name - String
                        string subAnimName = BinaryConverter.ByteToString(reader.ReadBytes(nameLength));
                        //Dummy bytes - 4 Bytes
                        int dummy = reader.ReadInt32();
                        //FPS - Int16
                        short fps = reader.ReadInt16();
                        //Frame count - Int16
                        short frames = reader.ReadInt16();
                        //Starting frame - Int16
                        short startingFrame = reader.ReadInt16();
                        //Ending frame - Int16
                        short endingFrame = reader.ReadInt16();
                        //Sub Anim item
                        SubAnim subAnim = new SubAnim
                        {
                            ID = subAnimName,
                            Dummy = dummy,
                            FPS = fps,
                            Frames = frames,
                            StartingFrame = startingFrame,
                            EndingFrame = endingFrame,
                            SubAnimTransforms = new List<SubAnimTransform>()
                        };
                        for (int j = 0; j < frames; j++)
                        {
                            //Transform key
                            byte key = reader.ReadByte();
                            //Key == 0x07 (Delete then create a new frame)
                            if (key == 0x07)
                            {
                                //TODO
                            }
                            //Total Transforms
                            byte transCount = reader.ReadByte();
                            //Key == 0x06 (Create new frame)
                            if (key == 0x06)
                            {

                            }
                            //Key == 0x04 (Keep using the current frame i guess)
                            else if (key == 0x04)
                            {

                            }
                            //Index in ref
                            short iRef = 0;
                            //Index ref num
                            short iRefNum = 0;
                            //Sub Anim num
                            short[] subAnimNum = new short[transCount];
                            for (byte k = 0; k < transCount; k++)
                            {
                                //Reference ID
                                byte refID = reader.ReadByte();
                                //Reference Type (0x00, 0x80, 0x90)
                                byte refType = reader.ReadByte();
                                //Reference Index
                                byte refIndex = reader.ReadByte();
                                //Declare
                                sats[i, iRefNum] = new SubAnimTransform
                                {
                                    RefID = refID,
                                    RefType = refType,
                                    RefIndex = refIndex,
                                    Transform = new Transform()
                                };
                                //Check Reference Type
                                //TODO: Get exact RefID's value
                                bool isTransformDetected = false;
                                if (refType == 0x80)
                                {
                                    //Declare
                                    if (sats[sats[i, iRefNum].RefIndex, iRef] == null)
                                        sats[sats[i, iRefNum].RefIndex, iRef] = new SubAnimTransform();
                                    //Take an declared transform
                                    iRef = 0;
                                    //Get transform type from a declared transform
                                    byte transType = sats[sats[i, iRefNum].RefIndex, iRef].Transform.Type;
                                    //Allowed transfrom types
                                    List<byte> allowedTransTypes = new List<byte> { 0x08, 0x18, 0x28, 0x38, 0x48, 0x68 };
                                    //Check if transform type is valid
                                    if (allowedTransTypes.Contains(transType))
                                    {
                                        //Copy it
                                        //sats[i, iRefNum] = sats[sats[i, iRefNum].RefIndex, iRef];
                                        //
                                        subAnimNum[k] = iRefNum;
                                        //
                                        iRef++;
                                        iRefNum++;
                                        //
                                        isTransformDetected = true;
                                    }
                                }
                                //
                                if (!isTransformDetected)
                                {
                                    iRefNum++;
                                    subAnimNum[k] = iRefNum;
                                }
                            }
                            //Total Transforms
                            transCount = reader.ReadByte();
                            //Index ref num
                            iRefNum = 0;
                            //
                            for (byte k = 0; k < transCount; k++)
                            {
                                //Index ID
                                byte indexID = reader.ReadByte();
                                //Temporary fix
                                if (sats[i, iRefNum] == null)
                                {
                                    sats[i, iRefNum] = new SubAnimTransform();
                                    sats[i, iRefNum].Transform = new Transform();
                                }
                                //Transform ID
                                sats[i, iRefNum].Transform.ID = indexID;
                                //
                                key = reader.ReadByte();
                                //
                                switch (key)
                                {
                                    case 0x08:
                                        sats[i, iRefNum].Transform.Type = key;
                                        sats[i, iRefNum].Transform.Point = new Point
                                        {
                                            Left = reader.ReadInt32(),
                                            Top = reader.ReadInt32()
                                        };
                                        break;
                                    case 0x18:
                                        sats[i, iRefNum].Transform.Type = key;
                                        sats[i, iRefNum].Transform.Matrix = new Matrix
                                        {
                                            X1 = (double)reader.ReadInt32() / 65536,
                                            Y1 = (double)reader.ReadInt32() / 65536,
                                            X2 = (double)reader.ReadInt32() / 65536,
                                            Y2 = (double)reader.ReadInt32() / 65536
                                        };
                                        sats[i, iRefNum].Transform.Point = new Point
                                        {
                                            Left = reader.ReadInt32(),
                                            Top = reader.ReadInt32()
                                        };
                                        break;
                                    case 0x28:
                                        sats[i, iRefNum].Transform.Type = key;
                                        sats[i, iRefNum].Transform.Point = new Point
                                        {
                                            Left = reader.ReadInt32(),
                                            Top = reader.ReadInt32()
                                        };
                                        sats[i, iRefNum].Transform.Color = new Color
                                        {
                                            Red = reader.ReadByte(),
                                            Green = reader.ReadByte(),
                                            Blue = reader.ReadByte(),
                                            Alpha = reader.ReadByte()
                                        };
                                        break;
                                    case 0x38:
                                        sats[i, iRefNum].Transform.Type = key;
                                        sats[i, iRefNum].Transform.Matrix = new Matrix
                                        {
                                            X1 = (double)reader.ReadInt32() / 65536,
                                            Y1 = (double)reader.ReadInt32() / 65536,
                                            X2 = (double)reader.ReadInt32() / 65536,
                                            Y2 = (double)reader.ReadInt32() / 65536
                                        };
                                        sats[i, iRefNum].Transform.Point = new Point
                                        {
                                            Left = reader.ReadInt32(),
                                            Top = reader.ReadInt32()
                                        };
                                        sats[i, iRefNum].Transform.Color = new Color()
                                        {
                                            Red = reader.ReadByte(),
                                            Green = reader.ReadByte(),
                                            Blue = reader.ReadByte(),
                                            Alpha = reader.ReadByte()
                                        };
                                        break;
                                    case 0x48:
                                        sats[i, iRefNum].Transform.Type = key;
                                        sats[i, iRefNum].Transform.Rotate = reader.ReadInt16();
                                        sats[i, iRefNum].Transform.Point = new Point
                                        {
                                            Left = reader.ReadInt32(),
                                            Top = reader.ReadInt32()
                                        };
                                        break;
                                    case 0x68:
                                        sats[i, iRefNum].Transform.Type = key;
                                        sats[i, iRefNum].Transform.Rotate = reader.ReadInt16();
                                        sats[i, iRefNum].Transform.Point = new Point
                                        {
                                            Left = reader.ReadInt32(),
                                            Top = reader.ReadInt32()
                                        };
                                        sats[i, iRefNum].Transform.Color = new Color()
                                        {
                                            Red = reader.ReadByte(),
                                            Green = reader.ReadByte(),
                                            Blue = reader.ReadByte(),
                                            Alpha = reader.ReadByte()
                                        };
                                        break;
                                    default:
                                        break;
                                }
                                //
                                if (k != transCount - 1)
                                {
                                    //
                                    if (subAnimNum[k + 1] - subAnimNum[k] > 1)
                                    {
                                        //
                                        for (short l = 0; l < subAnimNum[k + 1] - subAnimNum[k] - 1; l++)
                                        {
                                            iRefNum++;
                                            sats[i, iRefNum].Transform = sats[i, iRefNum - 1].Transform;
                                        }
                                    }
                                }
                                subAnim.SubAnimTransforms.Add(sats[i, iRefNum]);
                                iRefNum++;
                            }
                        }
                        //Add to collection
                        anim.SubAnims.Add(subAnim);
                    }
                }
            }
            return anim;
        }

        public void SaveRecord()
        {
            PopAnim sprites = ReadPam();
            using (StreamWriter file = File.CreateText(@"record.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(file, sprites);
            }
        }
    }
}

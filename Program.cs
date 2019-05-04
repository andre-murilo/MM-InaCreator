using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Generate_Mob_INA
{
    class Program
    {
        const int ANIMATIONS_COUNT = 6;
        const byte LAYER_ACTIVE_NUM = 1;
        const byte MAX_LAYERS = 8;
        const uint INA_FILE_ASSIGNATURE = 268435457;

        const int DIRECTIONS_MAX_COUNT = 8;
        const int DEFAULT_DIRECTION_COUNT = 5;

        public struct InaProp
        {
            public string file_name;
            public int iddle_frames;
            public int walk_frames;
            public int attack_frames;
            public int defense_frames;
            public int deathing_frames;
            public int death_frames;

            public int getTotalFrames()
            {
                return this.iddle_frames + this.walk_frames + this.attack_frames + this.defense_frames + this.deathing_frames + this.death_frames;
            }

            public int getAnimationLength(int index)
            {
                int result = 0;
                switch(index)
                {
                    case 0:
                        result = iddle_frames * 3;
                        break;
                    case 1:
                        result = walk_frames * 2;
                        break;
                    case 2:
                        result = attack_frames * 2;
                        break;
                    case 3:
                        result = defense_frames * 3;
                        break;
                    case 4:
                        result = deathing_frames * 3;
                        break;
                    case 5:
                        result = death_frames * 2;
                        break;
                }
                return result;
            }

            public int[] getAllowDirByAnimation(int animIndex)
            {
                int[] value = new int[8];
                switch(animIndex)
                {
                    case 0:
                        value[0] = 0;
                        value[1] = 0;
                        value[2] = 0;
                        value[3] = 0;
                        value[4] = 0;
                        value[5] = 0;
                        value[6] = 0;
                        value[7] = 0;
                        break;
                    case 1:
                        value[0] = 0;
                        value[1] = 0;
                        value[2] = 0;
                        value[3] = 0;
                        value[4] = 0;
                        value[5] = 0;
                        value[6] = 0;
                        value[7] = 0;
                        break;
                    case 2:
                        value[0] = 1;
                        value[1] = 0;
                        value[2] = 1;
                        value[3] = 0;
                        value[4] = 1;
                        value[5] = 0;
                        value[6] = 1;
                        value[7] = 0;
                        break;
                    case 3:
                        value[0] = 1;
                        value[1] = 0;
                        value[2] = 1;
                        value[3] = 0;
                        value[4] = 1;
                        value[5] = 0;
                        value[6] = 1;
                        value[7] = 0;
                        break;
                    case 4:
                        value[0] = 1;
                        value[1] = 0;
                        value[2] = 1;
                        value[3] = 0;
                        value[4] = 1;
                        value[5] = 0;
                        value[6] = 1;
                        value[7] = 0;
                        break;
                    case 5:
                        value[0] = 1;
                        value[1] = 0;
                        value[2] = 1;
                        value[3] = 0;
                        value[4] = 1;
                        value[5] = 0;
                        value[6] = 1;
                        value[7] = 0;
                        break;
                }
                return value;
            }

            public int getRangeAnim(int animIndex)
            {
                int result = 0;
                switch (animIndex)
                {
                    case 0:
                        result = 3;
                        break;
                    case 1:
                        result = 2;
                        break;
                    case 2:
                        result = 2;
                        break;
                    case 3:
                        result = 3;
                        break;
                    case 4:
                        result = 3;
                        break;
                    case 5:
                        result = 2;
                        break;
                }
                return result;
            }
        }


        private static InaProp GetInitialInfo()
        {
            Console.Write("Digite o nome do arquivo: ");
            string file_name = Console.ReadLine();
            if (!IsInputFileNameIsValid(file_name))
            {
                Console.WriteLine("[-] Nome de arquivo invalido, ou já existente!");
                Console.Read();
                Environment.Exit(0);
            }


            Console.Write("Nº iddle frames: ");
            int iddle_frames = int.Parse(Console.ReadLine());

            Console.Write("Nº walk frames: ");
            int walk_frames = int.Parse(Console.ReadLine());

            Console.Write("Nº attack frames: ");
            int attack_frames = int.Parse(Console.ReadLine());

            Console.Write("Nº defense frames: ");
            int defense_frames = int.Parse(Console.ReadLine());

            Console.Write("Nº deathing frames: ");
            int deathing_frames = int.Parse(Console.ReadLine());

            Console.Write("Nº death frames: ");
            int death_frames = int.Parse(Console.ReadLine());


            InaProp prop = new InaProp();
            prop.file_name = file_name;
            prop.iddle_frames = iddle_frames;
            prop.walk_frames = walk_frames;
            prop.attack_frames = attack_frames;
            prop.defense_frames = defense_frames;
            prop.deathing_frames = deathing_frames;
            prop.death_frames = death_frames;

            return prop;
        }


        private static byte[] getLayerEmpty()
        {
            byte[] Layer_empty = new byte[] {
                0x4c, 0x61, 0x79, 0x65, 0x72, 0x20, 0x31, 0x20, 0x28, 0x20, 0x69, 0x6d, 0x61, 0x67, 0x65, 0x20,
                0xfd, 0xfd, 0xfd, 0xfd, 0x20, 0x29, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00
            };
            return Layer_empty;
        }

        private static byte[] getLayerInUse(string name)
        {
            byte[] nulls = new byte[50];
            for(int i = 0; i < name.Length; i++)
            {
                nulls[i] = (byte)name[i];
            }
            return nulls;
        }
        private static bool IsInputFileNameIsValid(string fileName)
        {
            if(fileName == string.Empty)
                return false;

            if (File.Exists(Directory.GetCurrentDirectory() + @"\" + fileName))
                return false;

            return true;
        }


        private static void WriteFileHeader(BinaryWriter bw, InaProp prop)
        {
            bw.Write((UInt32)268435457);
            
            // write layers_active
            for(int i = 0; i < MAX_LAYERS; i++)
            {
                if (i == LAYER_ACTIVE_NUM)
                    bw.Write((byte)1);
                else
                    bw.Write((byte)0);
            }

            // write layers_info
            for(int i = 0; i < MAX_LAYERS; i++)
            {
                if(i == LAYER_ACTIVE_NUM)
                {
                    byte[] layer_use = getLayerInUse(prop.file_name);
                    bw.Write(layer_use);
                }
                else
                {
                    byte[] layer_empty = getLayerEmpty();
                    bw.Write(layer_empty);
                }
            }


            // write total frames per direction
            // total_frames = iddle + walk + attack + defense + deathing + death
            for (int i = 0; i < MAX_LAYERS; i++)
            {
                if (i == LAYER_ACTIVE_NUM)
                    bw.Write((int)prop.getTotalFrames());
                else
                    bw.Write((int)0);
            }

            
            
            
            // write animations qnt
            bw.Write((int)ANIMATIONS_COUNT);
            // write direct faces in animation qnt
            bw.Write((int)DIRECTIONS_MAX_COUNT);

           

            // write faces count by layers count
            for (int i = 0; i < MAX_LAYERS; i++)
            {
                if (i == LAYER_ACTIVE_NUM)
                    bw.Write((int)DEFAULT_DIRECTION_COUNT);
                else
                    bw.Write((int)0);
            }

        }

        private static void WriteFileContent(BinaryWriter bw, InaProp prop)
        {
            int current_frame = 0;

            for(int i = 0; i < ANIMATIONS_COUNT; i++)
            {
                // write current animation index
                bw.Write((int)i);
                // write current animation length
                bw.Write((int)prop.getAnimationLength(i));

                // write allowed faces in the current animation
                int[] allowedFaces = prop.getAllowDirByAnimation(i);
                for(int j = 0; j < allowedFaces.Length; j++)
                {
                    bw.Write((int)allowedFaces[j]);
                }

                // write all animation data
                int IsNextFrame = prop.getRangeAnim(i);
                int currentCounter = 0;
                for(int j = 0; j < prop.getAnimationLength(i); j++)
                {
                    // write unknow data
                    short[] unk = new short[5];
                    for(int k = 0; k < unk.Length; k++)
                    {
                        unk[k] = -1;
                        bw.Write((short)unk[k]);
                    }

                    // write animation in layers
                    for(int k = 0; k < MAX_LAYERS; k++)
                    {
                        for(int kk = 0; kk < 8; kk++)
                        {
                            if(k == LAYER_ACTIVE_NUM)
                            {
                                bw.Write((byte)1);
                            }
                            else
                            {
                                bw.Write((byte)0);
                            }
                        }

                        if(k != LAYER_ACTIVE_NUM)
                        {
                            bw.Write((short)-1);
                            bw.Write((byte)0);
                            bw.Write((byte)0);
                            bw.Write((byte)0);
                            bw.Write((byte)0);
                            bw.Write((byte)0);
                        }
                        else
                        {
                            bw.Write((short)current_frame); // current frame
                            bw.Write((byte)0);
                            bw.Write((byte)0);
                            bw.Write((byte)0);
                            bw.Write((byte)0);
                            bw.Write((byte)32);

                            currentCounter++;

                            if(currentCounter >= IsNextFrame)
                            {
                                current_frame++;
                                currentCounter = 0;
                            }

                        }



                    }



                }


            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("----- DigiMaster .INA animation generator -----\n");
            InaProp prop = GetInitialInfo();


            Console.WriteLine($"[+] Creating file: {prop.file_name}.ina");
            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8);

            WriteFileHeader(bw, prop);
            WriteFileContent(bw, prop);



            // get stream size
            int streamLen = (int)bw.BaseStream.Length;
            byte[] buffer = stream.GetBuffer();
            Array.Resize(ref buffer, streamLen);
            Console.WriteLine("Stream len: " + buffer.Length);

            // write buffer in file
            string base_dir = Directory.GetCurrentDirectory();
            File.WriteAllBytes(base_dir + @"\" + prop.file_name + ".ina", buffer);

            // close stream & binaryWriter
            bw.Close();
            stream.Close();

            // wait for exit this current process
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}

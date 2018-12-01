using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CryptoGraphyTest
{
    public partial class frmMain
    {
        private byte[] Seed = new byte[8];
        private byte[] IV = new byte[8];

        private void ClearAll()
        {
            textBox1.Text = textBox2.Text = "";
        }

        private void InitSeed()
        {
            Seed = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(textBox3.Text));
            IV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(textBox3.Text));
            //var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(textBox3.Text));
            //Array.Copy(data, Seed, Seed.Length);
            //Array.Copy(data, Seed.Length, IV, 0, IV.Length);
        }

        private void AESTest()
        {
            InitSeed();
            //AES每次128 bit (Key / IV / Block都是)
            var encryptor = Aes.Create().CreateEncryptor(Seed, IV);
            var decryptor = Aes.Create().CreateDecryptor(Seed, IV);
            byte[] input = Encoding.UTF8.GetBytes(textBox1.Text);
            byte[] encrypted_input = null;

            //對稱式加密法有block size對齊，這個還要補0很麻煩，不如直接
            //塞進MemoryStream去處理省工夫，反正加解密也沒有解一半的，都是直接解完整條
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(input, 0, input.Length);
                    cs.FlushFinalBlock();
                    textBox2.Text = Encoding.UTF8.GetString(ms.ToArray());
                }
                encrypted_input = ms.ToArray();
            }
            using (MemoryStream ms = new MemoryStream(encrypted_input))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        textBox4.Text = reader.ReadToEnd();
                    }
                }
            }
        }
        private void AESCngTest()
        { }
        private void SHA1Test()
        { }
        private void SHA256Test()
        { }
        private void DESTest()
        {
            InitSeed();
            var encryptor = DES.Create().CreateEncryptor(Seed, IV);
            var decryptor = DES.Create().CreateDecryptor(Seed, IV);
            byte[] input = Encoding.UTF8.GetBytes(textBox1.Text);
            byte[] encrypted_input = null; ;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(input, 0, input.Length);
                    cs.FlushFinalBlock();
                    textBox2.Text = Encoding.UTF8.GetString(ms.ToArray());
                }
                encrypted_input = ms.ToArray();
            }
        }
    }
}

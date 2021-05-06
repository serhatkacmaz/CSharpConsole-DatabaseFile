using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{

    class Program
    {
        static string file = Environment.CurrentDirectory + @"\students.dat"; //dosyanın yolunu exe ile aynı konumda olması  
        static FileStream stream;

        static void Main(string[] args)
        {
            // dosya yoksa, yanlışlıkla silindiysa boş dosya oluştur
            if (!File.Exists(file))
            {
                stream = File.Create(file);
                stream.Flush();
                stream.Close();
            }

            string name, surname, number, age, department, gender, birthplace, phone_number;
            int choice;

            // Menude çikişa basılana kadar menuyu getir
            while (true)
            {
                Console.WriteLine("MENU");
                Console.WriteLine("*********************");
                Console.WriteLine("1-Kayıt Ekle\n2-Kayıtları Listele\n3-Kayıt Ara\n4-Kayıt Düzenle\n5-Çıkış\nSeçim Yapınız...");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 1:// Ekleme
                        Console.Write("Ad: ");
                        name = Console.ReadLine();
                        Console.Write("Soyad: ");
                        surname = Console.ReadLine();
                        Console.Write("Yaş: ");
                        age = Console.ReadLine();
                        Console.Write("Numara: ");
                        number = Console.ReadLine();
                        Console.Write("Bölümü: ");
                        department = Console.ReadLine();
                        Console.Write("Cinsiyet: ");
                        gender = Console.ReadLine();
                        Console.Write("Doğum Yeri: ");
                        birthplace = Console.ReadLine();
                        Console.Write("Telefon Numara: ");
                        phone_number = Console.ReadLine();
                        Insert(name.ToUpper(), surname.ToUpper(), age, number, department.ToUpper(), gender.ToUpper(), birthplace.ToUpper(), phone_number);
                        break;
                    case 2:// Listeleme 
                        Console.WriteLine("1-Cinsiyet, 2-Yaş, 3-Bölüm, 4-Hepsi, Hangi seçeneğe göre listeleme yapılsın? ");
                        choice = Convert.ToInt16(Console.ReadLine());
                        string key;
                        switch (choice)
                        {
                            case 1:
                                Console.Write("Cinsiyet: ");
                                key = Console.ReadLine();
                                List(key.ToUpper());
                                break;
                            case 2:
                                Console.Write("Yaş: ");
                                key = Console.ReadLine();
                                List(key);
                                break;
                            case 3:
                                Console.Write("Bölüm: ");
                                key = Console.ReadLine();
                                List(key.ToUpper());
                                break;
                            case 4:
                                List();
                                break;
                            default:
                                Console.WriteLine("Hatalı Giriş!!!");
                                break;
                        }
                        break;
                    case 3:// Arama 
                        Console.WriteLine("Arama Yapmak için Numara ve Soyadı Giriniz");
                        Console.Write("Numara: ");
                        string key1 = Console.ReadLine();
                        Console.Write("Soyad: ");
                        string key2 = Console.ReadLine();
                        Search(key1.ToUpper(), key2.ToUpper());
                        break;
                    case 4:// Düzenleme
                        Console.WriteLine("Kayıt Düzenleme için Soyad ve Numara Giriniz");
                        Console.Write("Soyad: ");
                        surname = Console.ReadLine();
                        Console.Write("Numara: ");
                        number = Console.ReadLine();
                        if (Search(number.ToUpper(), surname.ToUpper()))
                        {
                            Console.WriteLine("Değiştirmek istediginiz değeri,yeni değeri giriniz.(eski_ad,yeni_ad) gibi");
                            string cevap = Console.ReadLine();

                            try
                            {
                                string eski = cevap.Substring(0, cevap.IndexOf(','));
                                string yeni = cevap.Substring(cevap.IndexOf(',') + 1);
                                Update(number, surname.ToUpper(), eski.ToUpper(), yeni.ToUpper());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Yanlış Yazdınız.");
                            }

                        }
                        else
                            Console.WriteLine("Güncelleme Olamaz.");
                        break;
                    case 5:// Cikis
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Hatalı Giriş!!!");
                        break;
                }

            }
            Console.ReadLine();
        }

        // Tüm Verileri Listeler
        static void List()
        {
            StreamReader sr = new StreamReader(file);
            string row = sr.ReadLine();
            if (row != null)
            {
                //satır satır okuma
                while (row != null)
                {
                    Console.WriteLine(row);
                    row = sr.ReadLine();
                }
            }
            else
                Console.WriteLine("Dosya Boş...");
            sr.Close();
        }

        // Degeri yazılan sütüna göre listeler
        static void List(string key)
        {
            StreamReader sr;
            if (key != "")
            {
                key += "-";
                sr = new StreamReader(file);
                string row = sr.ReadLine();
                if (row == null)
                    Console.WriteLine("Dosya Boş...");
                while (row != null)
                {
                    //istenilen filtre varsa ekrana bas
                    if (row.Contains(key))
                        Console.WriteLine(row);
                    row = sr.ReadLine();
                }
                sr.Close();
            }
            else
                Console.WriteLine("Değer Girilmedi...");
        }

        // Aranan kişi varsa ekrana bas
        static bool Search(string number, string surname)
        {
            StreamReader sr;
            if (number != "" && surname != "")
            {
                bool state = false;
                number += "-";
                surname += "-";
                sr = new StreamReader(file);
                string row = sr.ReadLine();
                if (row == null)
                    Console.WriteLine("Dosya Boş...");
                //dosya içindeki satırları teker teker oku ve if şartını kontrol et
                while (row != null)
                {
                    if (row.Contains(number) && row.Contains(surname))
                    {
                        state = true;
                        break;
                    }
                    row = sr.ReadLine();
                }
                sr.Close();
                if (state)
                {
                    Console.WriteLine(row);
                    return true;
                }
                else
                    Console.WriteLine("Aran Kişi Yok...");
            }
            else
                Console.WriteLine("Değerlerden en az biri boş girildi...");
            return false;
        }

        // Düzenleme, numara ve soyadı girilen kişinin istenilen bilgisini güncelleme
        static void Update(string number, string surname, string old, string newly)
        {
            string text = "";
            if (old != "" && newly != "")
            {
                number += "-";
                surname += "-";
                StreamReader sr = new StreamReader(file);
                string row = sr.ReadLine();
                //satır satır dosayayı null olana kadar oku
                while (row != null)
                {
                    if (row.Contains(number) && row.Contains(surname))
                    {
                        string[] word = row.Split('-');         //satırdaki bilgileri diziye atma
                        //eski veriyi yeni veriyle değiştirme
                        for (int i = 0; i < word.Length; i++)
                        {
                            if (word[i] == old)
                            {
                                word[i] = newly;
                                break;
                            }
                        }
                        for (int i = 0; i < word.Length; i++)
                        {
                            if (i < word.Length - 1)
                                text += word[i] + "-";
                            else
                                text += word[i] + "\n";
                        }
                        row = sr.ReadLine();
                    }
                    else
                    {
                        text += row + "\n";
                        row = sr.ReadLine();
                    }
                }
                sr.Close();

                Console.WriteLine("Güncelldendi");
                File.Delete(file);
                File.AppendAllText(file, text);

            }
            else
                Console.WriteLine("Değerlerden en az biri boş girildi...");
        }

        // Dosya içine veri kaydetme 
        static void Insert(string name, string surnname, string age, string number, string department, string gender, string birthplace, string phone_number)
        {
            string satir = (name + "-" + surnname + "-" + age + "-" + number + "-" + department + "-" + gender + "-" + birthplace + "-" + phone_number + "\n");
            File.AppendAllText(file, satir);
            Console.WriteLine("Ekleme Yapildi..\n");
        }
    }
}

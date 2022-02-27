using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace Projekt3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Quiz> kerdesek = new List<Quiz>();
        List<Quiz> aktív = new List<Quiz>();
        string[] sor = File.ReadAllLines("kerdesek.txt");
        int counter = 0;
        int temakSzama = 0;
        public MainWindow()
        {
            InitializeComponent();
            kiertekeles.Visibility = Visibility.Hidden;
            KerdesekFeltoltese();
            KivalasztottElem();
            Frissites();
            MessageBox.Show("Válasszon témát!");
        }
        private void KerdesekFeltoltese()
        {
            for (int i = 0; i < sor.Length; i++)
            {
                kerdesek.Add(new Quiz(0, sor[i]));
            }
            for (int i = 0; i < kerdesek.Count; i++)
            {
                if (!temak.Items.Contains(kerdesek[i].Topic))
                {
                    temak.Items.Add(kerdesek[i].Topic);
                    temakSzama++;
                }
            }
        }
        private void bal_Click(object sender, RoutedEventArgs e)
        {
            if (counter != 0)
                counter--;
            elerhetoKerdesek.Content = $"{counter + 1}/{aktív.Count}";
            Frissites();
        }
        private void jobb_Click(object sender, RoutedEventArgs e)
        {
            if(counter + 1 < aktív.Count)
                counter++;
            elerhetoKerdesek.Content = $"{counter + 1}/{aktív.Count}";
            Frissites();
        }
        private void kiretekeles_Click(object sender, RoutedEventArgs e)
        {
            int helyesValaszok = 0;
            for (int i = 0; i < aktív.Count; i++)
            {
                if (int.Parse(aktív[i].Helyes) == aktív[i].jelolt)
                    helyesValaszok++;
            }
            MessageBox.Show($"Kérdések száma: {aktív.Count}\nHelyes válaszok száma: {helyesValaszok}");
            Reset();
        }
        private void Ellenorzes()
        {
            int i = 0;
            bool jelolesek = false;
            do
            {
                if (aktív[i].jelolt == 0)
                    jelolesek = true;
                i++;
            } while (jelolesek != true && aktív.Count != i);
            if (jelolesek == true)
                kiertekeles.Visibility = Visibility.Hidden;
            else
                kiertekeles.Visibility = Visibility.Visible;
        }
        private void Frissites()
        {
            kerdes.Text = aktív[counter].Kerdes;
            valasz1.Content = aktív[counter].V1;
            valasz2.Content = aktív[counter].V2;
            valasz3.Content = aktív[counter].V3;
            valasz4.Content = aktív[counter].V4;
            Ellenorzes();
            if (aktív[counter].jelolt == 1)
                valasz1.IsChecked = true;
            else if(aktív[counter].jelolt == 2)
                valasz2.IsChecked = true;
            else if (aktív[counter].jelolt == 3)
                valasz3.IsChecked = true;
            else if (aktív[counter].jelolt == 4)
                valasz4.IsChecked = true;
            else
            {
                valasz1.IsChecked = false;
                valasz2.IsChecked = false;
                valasz3.IsChecked = false;
                valasz4.IsChecked = false;
            }
        }
        private void KivalasztottElem()
        {
            if (temak.SelectedItem as ComboBoxItem == (object)"Gaming")
            {
                for (int i = 0; i < kerdesek.Count; i++)
                {
                    if (kerdesek[i].Topic == "Gaming" && !aktív.Contains(kerdesek[i]))
                        aktív.Add(kerdesek[i]);
                }
                Frissites();
            }
            else if (temak.SelectedItem as ComboBoxItem == (object)"Történelem")
            {
                for (int i = 0; i < kerdesek.Count; i++)
                {
                    if (kerdesek[i].Topic == "Történelem" && !aktív.Contains(kerdesek[i]))
                        aktív.Add(kerdesek[i]);
                }
                Frissites();
            }
            else if (temak.SelectedItem as ComboBoxItem == (object)"Tech")
            {
                for (int i = 0; i < kerdesek.Count; i++)
                {
                    if (kerdesek[i].Topic == "Tech" && !aktív.Contains(kerdesek[i]))
                        aktív.Add(kerdesek[i]);
                }
                Frissites();
            }
            else
            {
                for (int i = 0; i < kerdesek.Count; i++)
                {
                    aktív.Add(kerdesek[i]);
                }
                Frissites();
            }
            elerhetoKerdesek.Content = $"{counter + 1}/{aktív.Count}";
        }
        private void valasz1_Checked(object sender, RoutedEventArgs e)
        {
            aktív[counter].jelolt = 1;
            Ellenorzes();
        }
        private void valasz2_Checked(object sender, RoutedEventArgs e)
        {
            aktív[counter].jelolt = 2;
            Ellenorzes();
        }
        private void valasz3_Checked(object sender, RoutedEventArgs e)
        {
            aktív[counter].jelolt = 3;
            Ellenorzes();
        }
        private void valasz4_Checked(object sender, RoutedEventArgs e)
        {
            aktív[counter].jelolt = 4;
            Ellenorzes();
        }
        private void Reset()
        {
            aktív.Clear();
            kerdesek.Clear();
            temak.SelectedItem = (object)"Összes";
            KerdesekFeltoltese();
            KivalasztottElem();
            counter = 0;
            elerhetoKerdesek.Content = $"{counter + 1}/{aktív.Count}";
            Frissites();
        }
        private void temak_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            KivalasztottElem();
            temakSzama++;
            teszt.Content = temakSzama;
        }
    }
    class Quiz
    {
        private string kerdes;
        private string topic;
        private string v1;
        private string v2;
        private string v3;
        private string v4;
        private string helyes;
        public int jelolt;
        public Quiz(int jelolt, string sor)
        {
            string[] sorElemi = sor.Split(';');
            kerdes = sorElemi[0];
            topic = sorElemi[1];
            v1 = sorElemi[2];
            v2 = sorElemi[3];
            v3 = sorElemi[4];
            v4 = sorElemi[5];
            helyes = sorElemi[6];
            jelolt = this.jelolt;
        }
        public string Kerdes { get { return kerdes; } }
        public string Topic { get { return topic; } }
        public string V1 { get { return v1; } }
        public string V2 { get { return v2; } }
        public string V3 { get { return v3; } }
        public string V4 { get { return v4; } }
        public string Helyes { get { return helyes; } }
    }
}
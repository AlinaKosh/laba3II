using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using Image = System.Windows.Controls.Image;

namespace HammingNetwork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        HammingNetwork net;
        List<string> names = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        //Нормализация входных параметров
        private double[] MyPars(string[] ps)
        {
            double[] res = new double[HammingNetwork.M];
            int a = 0;

            //Место в мире по площади выращивания винограда
            names.Add(ps[0]);

            //Место в мире по численности населения винодельцев 
            byte p1 = byte.Parse(ps[1]);
            for (int j = 0; j < 8; j++, a++)
            {
                res[a] = (p1 & 1) == 1 ? 1 : -1; // true это 1 - false это -1
                p1 >>= 1;
            }

            
            byte p2 = byte.Parse(ps[2]);
            for (int j = 0; j < 8; j++, a++)
            {
                res[a] = (p2 & 1) == 1 ? 1 : -1;
                p2 >>= 1;
            }

            //Форма организации винодельческого сообщества 
            res[a] = "Корпорации и семейные винодельни".Equals(ps[3]) ? 1 : -1;
            a++;

            //Организация винодельческих регионов 
            res[a] = "Контроль происхождения и качества вина".Equals(ps[4]) ? 1 : -1;
            a++;

            //Географическое положение винодельческой территории 
            res[a] = "Европа".Equals(ps[5]) || "Евразия".Equals(ps[5]) ? 1 : -1; a++;
            res[a] = "Aзия".Equals(ps[5]) || "Евразия".Equals(ps[5]) ? 1 : -1; a++;
            res[a] = "Северная Америка".Equals(ps[5]) ? 1 : -1; a++;
            res[a] = "Южная Америка".Equals(ps[5]) ? 1 : -1; a++;
            res[a] = "Африка".Equals(ps[5]) ? 1 : -1; a++;
            res[a] = "Австралия".Equals(ps[5]) ? 1 : -1; a++;

            //Уровень развития винодельческой инфраструктуры 
            res[a] = "низкий".Equals(ps[6]) ? 1 : -1; a++;
            res[a] = "средний".Equals(ps[6]) ? 1 : -1; a++;
            res[a] = "высокий".Equals(ps[6]) ? 1 : -1; a++;
            res[a] = "очень высокий".Equals(ps[6]) ? 1 : -1; a++;

            //Индекс винодельческого развития 
            res[a] = "низкий".Equals(ps[7]) ? 1 : -1; a++;
            res[a] = "средний".Equals(ps[7]) ? 1 : -1; a++;
            res[a] = "высокий".Equals(ps[7]) ? 1 : -1; a++;
            res[a] = "очень высокий".Equals(ps[7]) ? 1 : -1; a++;

            //Место в мире по объему винодельческой экономики 
            byte p8 = byte.Parse(ps[8]);
            for (int j = 0; j < 8; j++, a++)
            {
                res[a] = (p8 & 1) == 1 ? 1 : -1;
                p8 >>= 1;
            }

            return res;
        }

        private double[,] GetData(string path)
        {
            double[,] xs = new double[HammingNetwork.K, HammingNetwork.M];

            using(StreamReader r = new StreamReader(path))
            {
                for (int i = 0; i < HammingNetwork.K; i++)
                { 
                    string[] line = r.ReadLine().Split('\t');
                    double[] res = MyPars(line);

                    for (int j = 0; j < HammingNetwork.M; j++) xs[i, j] = res[j];
                }
            }

            return xs;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HammingNetwork.M = int.Parse(M.Text);
            HammingNetwork.K = int.Parse(K.Text);
            HammingNetwork.E = double.Parse(E.Text);
            HammingNetwork.Emin = double.Parse(Emin.Text);
            HammingNetwork.T = double.Parse(M.Text) / 2;

            string path = Path.Text;
            double[,] xs = GetData(path);

            net = new HammingNetwork(xs);

            MessageBox.Show("Сеть создана!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            double[] xs = MyPars(new string[] {
                What.Text,
                Size.Text,
                People.Text,
                Type1.Text,
                Type2.Text,
                Place.Text,
                Urban.Text,
                IPD.Text,
                GDP.Text
            });
            
            double[] res = net.Find(xs);

            string name = "";
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] > 0) name += this.names[i] + " ";
            }

            Out.Text = name;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path = Path.Text;
            int n = int.Parse(N.Text);

            string r = File.ReadLines(path).Skip(n - 1).First();
            string[] rs = r.Split('\t');

            What.Text = "Введенный образ: " + rs[0];
            Size.Text = rs[1];
            People.Text = rs[2];
            Type1.Text = rs[3];
            Type2.Text = rs[4];
            Place.Text = rs[5];
            Urban.Text = rs[6];
            IPD.Text = rs[7];
            GDP.Text = rs[8];
        }

        private void Path_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

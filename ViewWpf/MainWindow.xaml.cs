﻿using System;
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
using Controllers;
using Controllers.DAL;

namespace ViewWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Context conn = new Context();
        List<string> CharacterList = new List<string>();
        private string Char1 = "";
        private string Char2 = "";
        private string Char3 = "";
        private int atualChanged = 0;
        private int charLoad = 0;
        public bool loginCheck { get; set; }
        MenuAdminWindow menuAdmin;

        public MainWindow()
        {
            InitializeComponent();
            CharacterList = conn.return_Characters();
        }

        //=====================Character====================

        private void Character_Select(object sender, MouseButtonEventArgs e)
        {
            if (loginCheck == false)
            {
                MessageBox.Show("Login to select a character");
            }
            if (e.ClickCount == 2 && loginCheck == true)
            {
                //MessageBox.Show("Double");
                Image image = (Image)sender;
                atualChanged += 1;
                if (atualChanged == 1)
                {
                    Char1 = image.Tag.ToString();
                    CharacterSelect1.Source = new BitmapImage(new Uri("Characters/" + Char1 + "/" + Char1 + "_default.png", UriKind.Relative));
                    CharacterSelect1.Tag = Char1;
                }
                else if (atualChanged == 2)
                {
                    Char2 = image.Tag.ToString();
                    CharacterSelect2.Source = new BitmapImage(new Uri("Characters/" + Char2 + "/" + Char2 + "_default.png", UriKind.Relative));
                    CharacterSelect2.Tag = Char2;
                }
                else if (atualChanged == 3)
                {
                    Char3 = image.Tag.ToString();
                    CharacterSelect3.Source = new BitmapImage(new Uri("Characters/" + Char3 + "/" + Char3 + "_default.png", UriKind.Relative));
                    CharacterSelect3.Tag = Char3;
                    atualChanged = 0;
                }
            }
        }

        private void Character_Load(object sender, RoutedEventArgs e)
        {
            Image Character = (Image)sender;
            if (CharacterList.Count > charLoad)
            {
                Character.Source = new BitmapImage(new Uri("Characters/" + CharacterList[charLoad] + "/" + CharacterList[charLoad] + "_default.png", UriKind.Relative));
                Character.Tag = CharacterList[charLoad];
                charLoad += 1;
            }
            else
            {
                Character.Source = new BitmapImage(new Uri("Others/invalid_default.png", UriKind.Relative));
            }
        }

        //=====================Account====================

        private void Create_Account(object sender, RoutedEventArgs e)
        {
            string name = RegisLogin.Text;
            string pass = RegisPass.Password;
            string passCon = RegisPassCon.Password;

            if (pass == passCon)
            {
                if (conn.registerAccount(name, pass) == true)
                {
                    MessageBox.Show("Account created");
                    Grid_Visibility(1);
                }
                else
                {
                    MessageBox.Show("Change login");
                }
            }
            else if (pass != passCon)
            {
                MessageBox.Show("Password dont match");
            }
        }

        private void Login_Account(object sender, RoutedEventArgs e)
        {
            string login = LoginText.Text;
            string pass = PassText.Password;
            if (login == "" || pass == "")
            {
                MessageBox.Show("Login or Password incorrect");
            }
            else
            {
                bool loginAut = conn.loginAuthentication(login, pass);
                if (loginAut == true)
                {
                    Grid_Visibility(sender, e);
                    StartButton.IsEnabled = true;
                    RandomButton.IsEnabled = true;
                    loginCheck = true;
                }
                else if (loginAut == false)
                {
                    MessageBox.Show("Login or Password incorrect");
                }
            }
        }

        //=====================Others====================

        private void Start_Battle(object sender, RoutedEventArgs e)
        {
            //precisa estar logado

            if (CharacterSelect1.Tag.ToString() == "" || CharacterSelect2.Tag.ToString() == "" || CharacterSelect3.Tag.ToString() == "")
            {
                MessageBox.Show("Select 3 characters");
            }
            else
            {
                string char1 = CharacterSelect1.Tag.ToString();
                string char2 = CharacterSelect2.Tag.ToString();
                string char3 = CharacterSelect3.Tag.ToString();
                BattleWindow btWin = new BattleWindow(char1, char2, char3);
                btWin.ShowDialog();
            }

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                menuAdmin.Close();
                this.Close();
            }
            catch
            {
                this.Close();
            }
            

            
        }

        private void Grid_Visibility(int grid /*1 login, 2 regis*/)
        {
            if (grid == 1)
            {
                GridLogin.Visibility = Visibility.Visible;
                GridRegis.Visibility = Visibility.Hidden;
            }
            if (grid == 2)
            {
                GridLogin.Visibility = Visibility.Hidden;
                GridRegis.Visibility = Visibility.Visible;
            }
            if (grid == 3)
            {
                SubGridLogin.Visibility = Visibility.Hidden;
                WelcomeMessage.Visibility = Visibility.Visible;
                GridCharacters.Visibility = Visibility.Visible;
            }

        }

        private void Grid_Visibility(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string btnContent = btn.Name;

            if (btnContent == "Return")
            {
                Grid_Visibility(1);
            }
            if (btnContent == "RegisterShow")
            {
                Grid_Visibility(2);
            }
            if (btnContent == "Login")
            {
                Grid_Visibility(3);
            }
        }

        private void Random_Characters(object sender, RoutedEventArgs e)
        {
            int charNumber = CharacterList.Count();
            Random random = new Random();
            int randomNum1;
            int randomNum2;
            int randomNum3;

            do
            {
                randomNum1 = random.Next(0, charNumber);
                randomNum2 = random.Next(0, charNumber);
                randomNum3 = random.Next(0, charNumber);

            } while (randomNum1 == randomNum2 || randomNum1 == randomNum3 || randomNum2 == randomNum3);

            CharacterSelect1.Source = new BitmapImage(new Uri("Characters/" + CharacterList[randomNum1] + "/" + CharacterList[randomNum1] + "_default.png", UriKind.Relative));
            CharacterSelect1.Tag = CharacterList[randomNum1];

            CharacterSelect2.Source = new BitmapImage(new Uri("Characters/" + CharacterList[randomNum2] + "/" + CharacterList[randomNum2] + "_default.png", UriKind.Relative));
            CharacterSelect2.Tag = CharacterList[randomNum2];

            CharacterSelect3.Source = new BitmapImage(new Uri("Characters/" + CharacterList[randomNum3] + "/" + CharacterList[randomNum3] + "_default.png", UriKind.Relative));
            CharacterSelect3.Tag = CharacterList[randomNum3];
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                menuAdmin = new MenuAdminWindow();
                menuAdmin.Show();
            }
        }

        //private void Character1_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Character1.Height = 97;
        //    Character1.Width = 97;
        //}

        //private void Character1_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Character1.Height = 87;
        //    Character1.Width = 87;
        //}
    }
}

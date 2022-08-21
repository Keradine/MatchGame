using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e) //Object Sender is a parameter called Sender that contains a reference to the control/object that raised the event.
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>() //create a list of eight pairs of emoji
            {
                "🙈", "🙈",
                "🐶", "🐶",
                "💫", "💫",
                "🦍", "🦍",
                "🦮", "🦮",
                "🐺", "🐺",
                "🦝", "🦝",
                "🐱", "🐱",
            };

            Random random = new Random(); //create a new random number generator, создает новый генератор случайных чисел

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>()) // Find every TextBlock in the main grid and repeat the following statement for each of them, находит каждый элемент TextBlock в сетке и повторяет следующие команды для каждого элемента
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count); // pick a random number between 0 and the number of emoji left in the list and call it "index", выбирает случайное число от 0 до количества эмодзи в списке и назначает ему имя "index"
                    string nextEmoji = animalEmoji[index]; //use the random number called "index" to get a random emoji from the list, использует случайное число с именем "index" для получания случайного эмодзи из списка
                    textBlock.Text = nextEmoji; //update the TextBlock with the random emoji from the list, обновляет TextBlock случайным эмодзи из списка
                    animalEmoji.RemoveAt(index); //remove the random emoji from the list.
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false; //этот признак определяет, щелкнул ли игрок на первом животном в паре, и теперь пытается найти для него пару

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false) //игрок только что щелкнул на первом животном в паре, поэтому это животное становится невидимым, а соответствующий элемент TextBlock сохраняется на случай, если его придется делать видимым снова;
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text) //игрок нашел пару, второе животное в паре становится невидимым, а признак findingMatch сбрасывается, чтобы следующее животное, на котором щелкнет игрок, снова считалось первым в паре;
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else //игрок щелкнул на животном, которое не совпадает с первым, поэтому первое выбранное животное снова становится видимым, а признак findingMatch сбрасывается;
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}

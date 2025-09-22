using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Lab_1_AsadullinaD.D._БПИ_23_01
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик, подключённый в XAML
        private void BtnExperience_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            if (!TryGetInputs(out string surname, out decimal salary, out int year)) return;

            DateTime hireDate = new DateTime(year, 1, 1);
            DateTime today = DateTime.Today;

            int yearsOfService = today.Year - hireDate.Year;
            // Если ещё не наступил день/месяц (1 янв) — корректируем (хотя для 1 янв это почти никогда)
            if (new DateTime(today.Year, 1, 1) > today)
                yearsOfService--;

            if (yearsOfService < 0) yearsOfService = 0;

            ResultExperience.Text = $"Сотрудник {surname}: стаж на {today:d} — {yearsOfService} полных лет.";
        }

        // Обработчик, добавленный в конструкторе
        private void BtnDays_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            if (!TryGetInputs(out string surname, out decimal salary, out int year)) return;

            DateTime hireDate = new DateTime(year, 1, 1);
            DateTime today = DateTime.Today;

            TimeSpan diff = today - hireDate;
            int days = diff.Days;
            if (days < 0) days = 0;

            ResultDays.Text = $"Сотрудник {surname}: с 1.1.{year} прошло {days} дней (по состоянию на {today:d}).";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            SurnameTextBox.Text = "";
            SalaryTextBox.Text = "";
            YearTextBox.Text = "";
            ClearResults();
        }

        private void ClearResults()
        {
            ResultExperience.Text = "";
            ResultDays.Text = "";
            ValidationText.Text = "";
        }

        // Универсальная проверка входных данных
        private bool TryGetInputs(out string surname, out decimal salary, out int year)
        {
            surname = SurnameTextBox.Text.Trim();
            salary = 0;
            year = 0;

            if (string.IsNullOrWhiteSpace(surname))
            {
                ValidationText.Text = "Ошибка: фамилия не должна быть пустой.";
                SurnameTextBox.Focus();
                return false;
            }

            // Проверяем оклад
            if (!decimal.TryParse(SalaryTextBox.Text.Trim(), out salary) || salary < 0)
            {
                ValidationText.Text = "Ошибка: неверный оклад. Введите положительное число.";
                SalaryTextBox.Focus();
                return false;
            }

            // Проверяем год
            if (!int.TryParse(YearTextBox.Text.Trim(), out year))
            {
                ValidationText.Text = "Ошибка: год введён некорректно.";
                YearTextBox.Focus();
                return false;
            }

            int currentYear = DateTime.Today.Year;
            if (year > currentYear)
            {
                ValidationText.Text = $"Ошибка: год должен быть до {currentYear}.";
                YearTextBox.Focus();
                return false;
            }

            return true;
        }

        // Валидация для поля "год" — только цифры
        private void YearTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        // Валидация для поля "оклад" — цифры и одна десятичная точка
        private void SalaryTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string current = SalaryTextBox.Text;
            string newText = current.Insert(SalaryTextBox.SelectionStart, e.Text);
            // Разрешаем цифры и точку/запятую; проверим что получится валидное decimal
            newText = newText.Replace(',', '.');
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9.,]$") || !decimal.TryParse(newText, out _);
        }

        

        // Общая защита: запрет ввода пробела
        private void Numeric_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}

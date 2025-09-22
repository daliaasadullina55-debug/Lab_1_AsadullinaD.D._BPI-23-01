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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Кнопка "Стаж"
        private void BtnExperience_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            if (!TryGetInputs(out string surname, out decimal salary, out int year)) return;

            // Создаем объект ClassW
            ClassW worker = new ClassW(surname, salary, year);
            int yearsOfService = worker.GetExperience();

            ResultExperience.Text = $"Сотрудник {worker.Surname}: стаж на {DateTime.Today:d} — {yearsOfService} полных лет.";
        }

        // Кнопка "Дни"
        private void BtnDays_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            if (!TryGetInputs(out string surname, out decimal salary, out int year)) return;

            // Создаем объект ClassW
            ClassW worker = new ClassW(surname, salary, year);
            int days = worker.GetDaysSinceHire();

            ResultDays.Text = $"Сотрудник {worker.Surname}: с 1.1.{year} прошло {days} дней (по состоянию на {DateTime.Today:d}).";
        }

        // Кнопка "Очистить"
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            SurnameTextBox.Text = "";
            SalaryTextBox.Text = "";
            YearTextBox.Text = "";
            ClearResults();
        }

        // Очистка результатов
        private void ClearResults()
        {
            ResultExperience.Text = "";
            ResultDays.Text = "";
            ValidationText.Text = "";
        }

        // Проверка входных данных
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

            if (!decimal.TryParse(SalaryTextBox.Text.Trim().Replace(',', '.'), out salary) || salary < 0)
            {
                ValidationText.Text = "Ошибка: неверный оклад. Введите положительное число.";
                SalaryTextBox.Focus();
                return false;
            }

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

        // Валидация ввода года — только цифры
        private void YearTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        // Валидация ввода оклада — цифры и одна точка/запятая
        private void SalaryTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string current = SalaryTextBox.Text;
            string newText = current.Insert(SalaryTextBox.SelectionStart, e.Text);
            newText = newText.Replace(',', '.');
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9.,]$") || !decimal.TryParse(newText, out _);
        }

        // Запрет на пробелы
        private void Numeric_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}

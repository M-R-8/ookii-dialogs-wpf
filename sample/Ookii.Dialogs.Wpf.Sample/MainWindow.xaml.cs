// Copyright 2009-2021 Ookii Dialogs Contributors
//
// Под лицензией BSD 3-Clause License («Лицензия»);
// вы не можете использовать этот файл, кроме как в соответствии с Лицензией.
// Вы можете получить копию Лицензии по адресу
//
// https://opensource.org/licenses/BSD-3-Clause
//
// Если это не требуется применимым законодательством или не согласовано в письменной форме, программное обеспечение
// распространяется по Лицензии распространяется на условиях «КАК ЕСТЬ»,
// БЕЗ КАКИХ-ЛИБО ГАРАНТИЙ ИЛИ УСЛОВИЙ, явных или подразумеваемых.
// См. Лицензию для конкретного языка, управляющего разрешениями и
// ограничения по Лицензии.

using System;
using System.Windows;
using System.Threading;
using System.ComponentModel;

namespace Ookii.Dialogs.Wpf.Sample
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// Реализует вывод различных системных окон.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProgressDialog _sampleProgressDialog = new ProgressDialog()
        {
            WindowTitle = "Progress dialog sample",
            Text = "This is a sample progress dialog...",
            Description = "Processing...",
            ShowTimeRemaining = true,
        };

        public MainWindow()
        {
            InitializeComponent();

            _sampleProgressDialog.DoWork += new DoWorkEventHandler(_sampleProgressDialog_DoWork);
        }

        /// <summary>
        /// Обработка события нажатия кнопки выбора диалогового окна.
        /// </summary>
        private void _showDialogButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_dialogComboBox.SelectedIndex)
            {
                case 0:
                    ShowTaskDialog();
                    break;
                case 1:
                    ShowTaskDialogWithCommandLinks();
                    break;
                case 2:
                    ShowProgressDialog();
                    break;
                case 3:
                    ShowCredentialDialog();
                    break;
                case 4:
                    ShowFolderBrowserDialog();
                    break;
                case 5:
                    ShowFolderBrowserDialogSelectMultiple();
                    break;
                case 6:
                    ShowOpenFileDialog();
                    break;
                case 7:
                    ShowSaveFileDialog();
                    break;
            }
        }

        /// <summary>
        /// Показывает окно "Диалог задач".
        /// </summary>
        private void ShowTaskDialog()
        {
            if (TaskDialog.OSSupportsTaskDialogs)
            {
                using (TaskDialog dialog = new TaskDialog())
                {
                    dialog.WindowTitle = "Task dialog sample";
                    dialog.MainInstruction = "This is an example task dialog.";
                    dialog.Content = "Task dialogs are a more flexible type of message box. Among other things, task dialogs support custom buttons, command links, scroll bars, expandable sections, radio buttons, a check box (useful for e.g. \"don't show this again\"), custom icons, and a footer. Some of those things are demonstrated here.";
                    dialog.ExpandedInformation = "Ookii.org's Task Dialog doesn't just provide a wrapper for the native Task Dialog API; it is designed to provide a programming interface that is natural to .Net developers.";
                    dialog.Footer = "Task Dialogs support footers and can even include <a href=\"http://www.ookii.org\">hyperlinks</a>.";
                    dialog.FooterIcon = TaskDialogIcon.Information;
                    dialog.EnableHyperlinks = true;
                    TaskDialogButton customButton = new TaskDialogButton("A custom button");
                    TaskDialogButton okButton = new TaskDialogButton(ButtonType.Ok);
                    TaskDialogButton cancelButton = new TaskDialogButton(ButtonType.Cancel);
                    dialog.Buttons.Add(customButton);
                    dialog.Buttons.Add(okButton);
                    dialog.Buttons.Add(cancelButton);
                    dialog.HyperlinkClicked += new EventHandler<HyperlinkClickedEventArgs>(TaskDialog_HyperLinkClicked);
                    TaskDialogButton button = dialog.ShowDialog(this);
                    if (button == customButton)
                        MessageBox.Show(this, "You clicked the custom button", "Task Dialog Sample");
                    else if (button == okButton)
                        MessageBox.Show(this, "You clicked the OK button.", "Task Dialog Sample");
                }
            }
            else
            {
                MessageBox.Show(this, "This operating system does not support task dialogs.", "Task Dialog Sample");
            }
        }

        /// <summary>
        /// Показывает диалоговое окно задачи со ссылками на команды.
        /// </summary>
        private void ShowTaskDialogWithCommandLinks()
        {
            if (TaskDialog.OSSupportsTaskDialogs)
            {
                using (TaskDialog dialog = new TaskDialog())
                {
                    dialog.WindowTitle = "Task dialog sample";
                    dialog.MainInstruction = "This is a sample task dialog with command links.";
                    dialog.Content = "Besides regular buttons, task dialogs also support command links. Only custom buttons are shown as command links; standard buttons remain regular buttons.";
                    dialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;
                    TaskDialogButton elevatedButton = new TaskDialogButton("An action requiring elevation");
                    elevatedButton.CommandLinkNote = "Both regular buttons and command links can show the shield icon to indicate that the action they perform requires elevation. It is up to the application to actually perform the elevation.";
                    elevatedButton.ElevationRequired = true;
                    TaskDialogButton otherButton = new TaskDialogButton("Some other action");
                    TaskDialogButton cancelButton = new TaskDialogButton(ButtonType.Cancel);
                    dialog.Buttons.Add(elevatedButton);
                    dialog.Buttons.Add(otherButton);
                    dialog.Buttons.Add(cancelButton);
                    dialog.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show(this, "This operating system does not support task dialogs.", "Task Dialog Sample");
            }
        }

        /// <summary>
        /// Показывает окно с процесс баром.
        /// </summary>
        private void ShowProgressDialog()
        {
            if (_sampleProgressDialog.IsBusy)
                MessageBox.Show(this, "The progress dialog is already displayed.", "Progress dialog sample");
            else
                _sampleProgressDialog.Show(); // Показать немодальный диалог; это рекомендуемый режим работы для диалогового окна прогресса.
        }

        /// <summary>
        /// Показывает диалоговое окно учетных данных.
        /// </summary>
        private void ShowCredentialDialog()
        {
            using (CredentialDialog dialog = new CredentialDialog())
            {
                // Заголовок окна не будет использоваться в Vista и более поздних версиях; там заголовок всегда будет «Безопасность Windows».
                dialog.WindowTitle = "Credential dialog sample";
                dialog.MainInstruction = "Please enter your username and password.";
                dialog.Content = "Since this is a sample the credentials won't be used for anything, so you can enter anything you like.";
                dialog.ShowSaveCheckBox = true;
                dialog.ShowUIForSavedCredentials = true;
                // Цель — это ключ, под которым будут храниться учетные данные.
                // Рекомендуется установить цель в соответствии с шаблоном "Company_Application_Server".
                // Цели указаны для пользователя, а не для приложения, поэтому использование такого шаблона обеспечит уникальность.
                dialog.Target = "Ookii_DialogsWpfSample_www.example.com";
                if (dialog.ShowDialog(this))
                {
                    MessageBox.Show(this, string.Format("You entered the following information:\nUser name: {0}\nPassword: {1}", dialog.Credentials.UserName, dialog.Credentials.Password), "Credential dialog sample");
                    // Обычно перед вызовом ConfirmCredentials следует проверить правильность учетных данных.
                    // ConfirmCredentials сохранит учетные данные тогда и только тогда, когда пользователь установил флажок сохранения.
                    dialog.ConfirmCredentials(true);
                }
            }
        }

        /// <summary>
        /// Показывает окно выбора папки.
        /// </summary>
        private void ShowFolderBrowserDialog()
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Please select a folder.";
            dialog.UseDescriptionForTitle = true; // Это относится только к диалоговому окну в стиле Vista, а не к старому диалоговому окну.

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            {
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            }

            if ((bool)dialog.ShowDialog(this))
            {
                MessageBox.Show(this, $"The selected folder was:{Environment.NewLine}{dialog.SelectedPath}", "Sample folder browser dialog");
            }
        }

        /// <summary>
        /// Показывает окно выбора нескольких папок.
        /// </summary>
        private void ShowFolderBrowserDialogSelectMultiple()
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Multiselect = true;
            dialog.Description = "Please select a folder.";
            dialog.UseDescriptionForTitle = true; // Это относится только к диалоговому окну в стиле Vista, а не к старому диалоговому окну.

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            {
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            }

            if ((bool)dialog.ShowDialog(this))
            {
                var selectedPaths = dialog.SelectedPaths;

                if (selectedPaths.Length == 1)
                {
                    MessageBox.Show(this, $"The selected folder was:{Environment.NewLine}{selectedPaths[0]}", "Sample folder browser dialog");
                }
                else
                {
                    MessageBox.Show(this, $"The selected folders were:{Environment.NewLine}{string.Join(Environment.NewLine, selectedPaths)}", "Sample folder browser dialog");
                }
            }
        }

        /// <summary>
        /// Показывает окно выбора файла.
        /// </summary>
        private void ShowOpenFileDialog()
        {
            // Начиная с .Net 3.5 SP1 класс WPF Microsoft.Win32.OpenFileDialog по-прежнему использует старый стиль
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*";
            if (!VistaFileDialog.IsVistaFileDialogSupported)
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular open file dialog will be used. Please use Windows Vista to see the new dialog.", "Sample open file dialog");
            if ((bool)dialog.ShowDialog(this))
                MessageBox.Show(this, "The selected file was: " + dialog.FileName, "Sample open file dialog");
        }

        /// <summary>
        /// Показывает окно сохранения файла.
        /// </summary>
        private void ShowSaveFileDialog()
        {
            VistaSaveFileDialog dialog = new VistaSaveFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.DefaultExt = "txt";
            // Начиная с .Net 3.5 SP1 класс WPF Microsoft.Win32.SaveFileDialog по-прежнему использует старый стиль
            if (!VistaFileDialog.IsVistaFileDialogSupported)
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular save file dialog will be used. Please use Windows Vista to see the new dialog.", "Sample save file dialog");
            if ((bool)dialog.ShowDialog(this))
                MessageBox.Show(this, "The selected file was: " + dialog.FileName, "Sample save file dialog");
        }

        private void TaskDialog_HyperLinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Href);
        }

        /// <summary>
        /// Реализация рассчета и вывода информации о показателе выполнения процесс бара.
        /// </summary>
        private void _sampleProgressDialog_DoWork(object sender, DoWorkEventArgs e)
        {
            // Реализуйте операцию, прогресс которой показывает индикатор выполнения, так же, как вы бы сделали это с фоновым исполнителем.
            for (int x = 0; x <= 100; ++x)
            {
                Thread.Sleep(500);

                // Периодически проверяем CancellationPending и при необходимости прерываем операцию.
                if (_sampleProgressDialog.CancellationPending)
                    return;

                // ReportProgress также может изменять основной текст и описание; передать null, чтобы оставить их без изменений.
                // Если для _sampleProgressDialog.ShowTimeRemaining установлено значение true, время будет автоматически рассчитываться на основе
                // частота вызовов ReportProgress.
                _sampleProgressDialog.ReportProgress(x, null, string.Format(System.Globalization.CultureInfo.CurrentCulture, "Processing: {0}%", x));
            }
        }
    }
}

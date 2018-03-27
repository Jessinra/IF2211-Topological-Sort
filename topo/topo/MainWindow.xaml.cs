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

namespace topo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void module_import_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("this is import module");
            if (!page_input.IsVisible)
            {
                page_default.Visibility = Visibility.Hidden;
                page_input.Visibility = Visibility.Visible;
                page_list.Visibility = Visibility.Hidden;
                page_dfs.Visibility = Visibility.Hidden;
                page_bfs.Visibility = Visibility.Hidden;
                page_info.Visibility = Visibility.Hidden;

                HL01.Visibility = Visibility.Visible;
                HL02.Visibility = Visibility.Hidden;
                HL03.Visibility = Visibility.Hidden;
                HL04.Visibility = Visibility.Hidden;
                HL05.Visibility = Visibility.Hidden;
                

            }
            else if (page_input.IsVisible)
            {
                page_default.Visibility = Visibility.Visible;
                page_input.Visibility = Visibility.Hidden;

                HL01.Visibility = Visibility.Hidden;
            }
            ListSubjectViewer.main();

            list_subject.Visibility = Visibility.Hidden;
            BFS_plan.Visibility = Visibility.Hidden;
            DFS_plan.Visibility = Visibility.Hidden;
            Info_box.Visibility = Visibility.Hidden;
            rtb_input.Visibility = Visibility.Visible;
            enter_button1.Visibility = Visibility.Visible;
        }

        private void module_list_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("this is list module");
            if (!page_list.IsVisible)
            {
                page_default.Visibility = Visibility.Hidden;
                page_input.Visibility = Visibility.Hidden;
                page_list.Visibility = Visibility.Visible;
                page_dfs.Visibility = Visibility.Hidden;
                page_bfs.Visibility = Visibility.Hidden;
                page_info.Visibility = Visibility.Hidden;

                HL01.Visibility = Visibility.Hidden;
                HL02.Visibility = Visibility.Visible;
                HL03.Visibility = Visibility.Hidden;
                HL04.Visibility = Visibility.Hidden;
                HL05.Visibility = Visibility.Hidden;

                list_subject.Visibility = Visibility.Visible;
                rtb_input.Visibility = Visibility.Hidden;

                string filename = ListSubjectViewer.filename;
                CoursePlan coursePlan = new CoursePlan(filename);
                list_subject.Text = coursePlan.get_name_list();

            }
            else if (page_list.IsVisible)
            {
                page_default.Visibility = Visibility.Visible;
                page_list.Visibility = Visibility.Hidden;

                HL02.Visibility = Visibility.Hidden;
            }

            BFS_plan.Visibility = Visibility.Hidden;
            DFS_plan.Visibility = Visibility.Hidden;
            Info_box.Visibility = Visibility.Hidden;
            enter_button1.Visibility = Visibility.Hidden;

        }

        private void module_dfs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("this is dfs module");
            if (!page_dfs.IsVisible)
            {
                page_default.Visibility = Visibility.Hidden;
                page_input.Visibility = Visibility.Hidden;
                page_list.Visibility = Visibility.Hidden;
                page_dfs.Visibility = Visibility.Visible;
                page_bfs.Visibility = Visibility.Hidden;
                page_info.Visibility = Visibility.Hidden;

                HL01.Visibility = Visibility.Hidden;
                HL02.Visibility = Visibility.Hidden;
                HL03.Visibility = Visibility.Visible;
                HL04.Visibility = Visibility.Hidden;
                HL05.Visibility = Visibility.Hidden;
            }
            else if (page_dfs.IsVisible)
            {
                page_default.Visibility = Visibility.Visible;
                page_dfs.Visibility = Visibility.Hidden;

                HL03.Visibility = Visibility.Hidden;
            }

            list_subject.Visibility = Visibility.Hidden;
            BFS_plan.Visibility = Visibility.Hidden;
            Info_box.Visibility = Visibility.Hidden;
            rtb_input.Visibility = Visibility.Hidden;
            enter_button1.Visibility = Visibility.Hidden;

            string filename = ListSubjectViewer.filename;
            CoursePlan coursePlan = new CoursePlan(filename);
            DFS dfsResult = new DFS(coursePlan.subjects);
            DFS_plan.Visibility = Visibility.Visible;
            DFS_plan.Text = dfsResult.get_name_list();
            

        }

        private void module_bfs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("this is bfs module");
            if (!page_bfs.IsVisible)
            {
                page_default.Visibility = Visibility.Hidden;
                page_input.Visibility = Visibility.Hidden;
                page_list.Visibility = Visibility.Hidden;
                page_dfs.Visibility = Visibility.Hidden;
                page_bfs.Visibility = Visibility.Visible;
                page_info.Visibility = Visibility.Hidden;

                HL01.Visibility = Visibility.Hidden;
                HL02.Visibility = Visibility.Hidden;
                HL03.Visibility = Visibility.Hidden;
                HL04.Visibility = Visibility.Visible;
                HL05.Visibility = Visibility.Hidden;
            }
            else if (page_bfs.IsVisible)
            {
                page_default.Visibility = Visibility.Visible;
                page_bfs.Visibility = Visibility.Hidden;

                HL04.Visibility = Visibility.Hidden;
            }

        
            list_subject.Visibility = Visibility.Hidden;
            DFS_plan.Visibility = Visibility.Hidden;
            Info_box.Visibility = Visibility.Hidden;
            rtb_input.Visibility = Visibility.Hidden;
            enter_button1.Visibility = Visibility.Hidden;

            string filename = ListSubjectViewer.filename;
            CoursePlan coursePlan = new CoursePlan(filename);
            BFS bfsResult = new BFS(coursePlan.subjects);
            BFS_plan.Visibility = Visibility.Visible;
            BFS_plan.Text = bfsResult.get_name_list();
            BFSViewer.main();
        }

        private void module_info_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("this is info module");
            if (!page_info.IsVisible)
            {
                page_default.Visibility = Visibility.Hidden;
                page_input.Visibility = Visibility.Hidden;
                page_list.Visibility = Visibility.Hidden;
                page_dfs.Visibility = Visibility.Hidden;
                page_bfs.Visibility = Visibility.Hidden;
                page_info.Visibility = Visibility.Visible;

                HL01.Visibility = Visibility.Hidden;
                HL02.Visibility = Visibility.Hidden;
                HL03.Visibility = Visibility.Hidden;
                HL04.Visibility = Visibility.Hidden;
                HL05.Visibility = Visibility.Visible;

                Info_box.Text =
                    "\n\n\n" +
                    "- Felix Septianus  (135 16 041)\n" +
                    "- Dicky Adrian     (135 16 050)\n" +
                    "- Jessin Donnyson  (135 16 050)\n" +
                    "\n\n" +
                    "Institut Teknologi Bandung\n" +
                    "All rights reserved 2018";

            }
            else if (page_info.IsVisible)
            {
                page_default.Visibility = Visibility.Visible;
                page_info.Visibility = Visibility.Hidden;

                HL05.Visibility = Visibility.Hidden;
            }

            list_subject.Visibility = Visibility.Hidden;
            BFS_plan.Visibility = Visibility.Hidden;
            DFS_plan.Visibility = Visibility.Hidden;
            Info_box.Visibility = Visibility.Visible;
            rtb_input.Visibility = Visibility.Hidden;
            enter_button1.Visibility = Visibility.Hidden;
        }

        private void list_subject_LostMouseCapture(object sender, MouseEventArgs e)
        {
            list_subject.Visibility = Visibility.Hidden;
            BFS_plan.Visibility = Visibility.Hidden;
            DFS_plan.Visibility = Visibility.Hidden;
            Info_box.Visibility = Visibility.Hidden;
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        private void EnterPressed(object sender, TouchEventArgs e)
        {

        }

        private void click_pressed(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("halo");
            TextRange textRange = new TextRange(rtb_input.Document.ContentStart, rtb_input.Document.ContentEnd);
            string content = textRange.Text;
            content = content.Replace("\r\n", "");
            Console.WriteLine(content);
            ListSubjectViewer.filename = content; BFSViewer.filename = content;
            
        }
    }
}

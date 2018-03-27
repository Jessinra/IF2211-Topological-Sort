using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Glee.Drawing;
using System.Drawing;

namespace topo
{
    public class ListSubjectViewer
    {
        public static string filename;

        public static void main()
        {
            //create a form
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.BackColor = System.Drawing.Color.Black;
            form.Size = new System.Drawing.Size(1000, 600);

            //create a viewer object
            Microsoft.Glee.GraphViewerGdi.GViewer viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();

            //create a graph object
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("graph");
            graph.GraphAttr.Backgroundcolor = Microsoft.Glee.Drawing.Color.LavenderBlush;

            //create the graph content
            CoursePlan coursePlan = new CoursePlan(filename);
            coursePlan.Print();

            foreach (KeyValuePair<string, Subject> entry in coursePlan.subjects)
            {
                Subject subject = entry.Value;
                foreach (string element in subject.preq_of)
                {
                    graph.AddEdge(subject.name, element);
                }
            }

            //bind the graph to the viewer
            viewer.Graph = graph;

            //associate the viewer with the form
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();

            //show the form
            form.ShowDialog();

            // print to console
            //DFS resultDFS = new DFS(coursePlan.subjects);
            //resultDFS.Print();
        }
    }

    public class BFSViewer
    {
        public static string filename;// = "../../../../" + "Daftar Kuliah.txt";

        public static void main()
        {
            //create a form
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Size = new System.Drawing.Size(1000, 600);
            
            //create a viewer object
            Microsoft.Glee.GraphViewerGdi.GViewer viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
            viewer.OutsideAreaBrush = Brushes.LightCoral;
            
            //create a graph object
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("graph");
            graph.GraphAttr.Backgroundcolor = Microsoft.Glee.Drawing.Color.LightCoral;
          
            //create the graph content
            CoursePlan coursePlan = new CoursePlan(filename);
            Subject tempSub = new Subject();
            BFS bfsResult = new BFS(coursePlan.subjects);
            CoursePlan tempCourse = new CoursePlan(filename);
            string temp = "1. " + bfsResult.result[1][0];
            List<string> convert = new List<string>();
            foreach(KeyValuePair<int, List<string>> entry in bfsResult.result)
            {
                foreach(string x in entry.Value)
                {
                    convert.Add(x);
                }
            }

            for (int i = 0; i < convert.Count-1; i++)
            {
                tempSub = tempCourse.subjects[convert[i]];
                foreach(string x in tempSub.preq_of)
                {
                    int index = convert.IndexOf(x) + 1;
                    graph.AddEdge(((i + 1).ToString() + ". " + convert[i]), (index.ToString() + ". " + x));
                }
            }
            int j = 1;
            foreach(KeyValuePair<int, List<string>> entry in bfsResult.result)
            {
                foreach (string tempp in entry.Value) {
                    Microsoft.Glee.Drawing.Node n = graph.FindNode(j.ToString() + ". " + tempp);
                    switch (entry.Key % 5)
                    {
                        case 1:
                            {
                                n.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.MidnightBlue;
                                n.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.LavenderBlush;
                                break;
                            }
                        case 2:
                            {
                                n.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.MediumBlue;
                                n.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.LavenderBlush;
                                break;
                            }
                        case 3:
                            {
                                n.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.RoyalBlue;
                                break;
                            }
                        case 4:
                            {
                                n.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.DodgerBlue;
                                break;
                            }
                        default:
                            {
                                n.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.SkyBlue;
                                break; 
                            }
                    }
                    j++;
                }
            }

            //bind the graph to the viewer
            viewer.Graph = graph;

            //associate the viewer with the form
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();

            //show the form
            form.ShowDialog();
        }
    }

    public struct Subject
    {
        public string name;
        public int n_of_preq;
        public bool isTaken;
        public List<string> preq_of;
    }

    public class CoursePlan
    {

        public Dictionary<string, Subject> subjects;

        public CoursePlan(string fileName)
        {
            // Create new Dictionary with string Subject's name as the key and Subject's info as the value
            subjects = new Dictionary<string, Subject>();

            // Create dummy to be the parent of all first semester subjects
            Subject dummySubject = new Subject();
            dummySubject.name = "AkuMasukITB";
            dummySubject.n_of_preq = -1;
            dummySubject.isTaken = false;
            dummySubject.preq_of = new List<string>();

            // Add dummy to dictionary
            subjects.Add("dummy", dummySubject);

            // Read file
            string[] subjectText = System.IO.File.ReadAllLines(fileName);

            // Add subject's name, subject's number of prerequisite, set isTaken to false, and create list of prerequisite
            foreach (string subjectData in subjectText)
            {
                string[] data = subjectData.Split(new string[] { ", ", "." }, StringSplitOptions.RemoveEmptyEntries);
                Subject subject = new Subject();

                subject.name = data[0];
                subject.n_of_preq = data.Length - 1;
                subject.isTaken = false;
                subject.preq_of = new List<string>();

                subjects.Add(data[0], subject);

                if (subject.n_of_preq == 0)
                {
                    subjects["dummy"].preq_of.Add(subject.name);
                }
            }

            // Fill the list of prerequisite of each subject
            foreach (string subjectData in subjectText)
            {
                string[] data = subjectData.Split(new string[] { ", ", "." }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < data.Length; i++)
                {
                    subjects[data[i]].preq_of.Add(data[0]);
                }
            }
        }

        public void Print()
        {
            foreach (KeyValuePair<string, Subject> entry in subjects)
            {
                Subject subject = entry.Value;
                Console.WriteLine("Mata Kuliah: {0}", subject.name);
                Console.WriteLine("N: {0}", subject.n_of_preq);
                Console.WriteLine("Taken? {0}", subject.isTaken);
                Console.Write("Prerequisite of: ");
                foreach (string preq in subject.preq_of)
                {
                    Console.Write(preq + " ");
                }
                Console.WriteLine("\n");
            }
        }
        public string get_name_list()
        {
            string result = "Mata Kuliah: \n\n";
            foreach (KeyValuePair<string, Subject> entry in subjects)
            {
                Subject subject = entry.Value;
                result = result + "- " + subject.name + "\n";

            }

            return result;
        }
    }
    /*
    class TopologicalSort
    {
        static void Main(string[] args)
        {
            CoursePlan coursePlan = new CoursePlan("Daftar Kuliah.txt");
            coursePlan.Print();

            DFS resultDFS = new DFS(coursePlan.subjects);
            resultDFS.Print();

            Console.WriteLine();
            BFS resultBFS = new BFS(coursePlan.subjects);
            resultBFS.Print();
        }
    }*/

    public class DFS
    {
        List<string> courseTaken;
        List<int> courseTaken_stamp;

        public DFS(Dictionary<string, Subject> subjects)
        {
            courseTaken = new List<string>();


            Sort(subjects, subjects["dummy"]);

            courseTaken.Reverse();
            

            List<string> result = new List<string>();
            List<int> result_stamp = new List<int>();

            int i = courseTaken.Count;
            foreach (string element in courseTaken)
            {
                
                if (element != "dummy" && !result.Contains(element))
                {
                    result.Add(element);
                    result_stamp.Add(i);

                }
                i--;
            }

            courseTaken = result;
            courseTaken_stamp = new List<int>();
            courseTaken_stamp = result_stamp;
        }
    
        public void Print()
        {
            Console.WriteLine("DFS");
            for (int i = 0; i < courseTaken.Count; i++)
            {
                Console.Write(courseTaken_stamp[i] + ": ");
                Console.WriteLine(courseTaken[i]);
            }
        }

        private void Sort(Dictionary<string, Subject> subjects, Subject currentSubject)
        {
            // if subject not taken yet
            if (!currentSubject.isTaken)
            {
                // take that subject
                currentSubject.isTaken = true;

                // add to taken list (move into node)
                courseTaken.Add(currentSubject.name);

                // Select next course
                if (isPrerequisite(currentSubject))
                {

                    List<string> nextCourses = currentSubject.preq_of;
                    foreach (string courseName in nextCourses)
                    {

                        Subject nextCourse = subjects[courseName];
                        Sort(subjects, nextCourse);
                    }
                }

                // add to taken list again (return from node)
                courseTaken.Add(currentSubject.name);

                // modify 
                subjects[currentSubject.name] = currentSubject;
            }
        }

        private bool isPrerequisite(Subject currentSubject)
        {
            // check if subject has anticedent that is not taken

            List<string> preq_of = currentSubject.preq_of;
            foreach (string course in preq_of)
            {

                // if theres courses to take, and not yet taken
                if (!(courseTaken.Contains(course)))
                {

                    return true;
                }
            }
            return false;
        }

        public string get_name_list()
        {

            string result_ = "Hasil DFS: \n\n";
            
            for (int i = 0; i < courseTaken.Count; i++)
            { 
                result_ = result_ + courseTaken_stamp[i] + ": " + courseTaken[i] + "\n";

            }

            return result_;
        }
    }

    public class BFS
    {
        public Dictionary<int, List<string>> result;

        public BFS(Dictionary<string, Subject> courses)
        {
            result = new Dictionary<int, List<string>>();

            int i = 1;
            List<string> pointedTo = new List<string>();
            while (canTakeCourse(courses))
            {
                pointedTo.Clear();
                List<string> takenList = new List<string>();
                foreach (KeyValuePair<string, Subject> entry in courses)
                {
                    Subject subject = entry.Value;
                    if (subject.n_of_preq == 0)
                    { //Taking course which have no prequisites
                        takenList.Add(subject.name);
                        foreach (string temp in subject.preq_of)
                        {
                            pointedTo.Add(temp);
                        }
                    }
                }
                foreach (string tempstring in pointedTo)
                { //Decrementing n_of_preq of from taken courses
                    Subject tempsub = courses[tempstring];
                    tempsub.n_of_preq--;
                    courses[tempstring] = tempsub;
                }

                foreach (string temp in takenList)
                { //remove taken course
                    courses.Remove(temp);
                }

                result.Add(i, takenList);
                i++;
            }
            
            if(courses.Count > 1)
            {
                List<string> x = new List<string>();
                x.Add("BFS Gagal");
                result.Clear();
                result.Add(0, x);
            }
        }

        bool canTakeCourse(Dictionary<string, Subject> courses)
        {
            //returns true if there is a course that can be taken
            foreach (KeyValuePair<string, Subject> entry in courses)
            {
                Subject subject = entry.Value;
                if (subject.n_of_preq == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void Print()
        {
            Console.WriteLine("BFS");
            foreach (KeyValuePair<int, List<string>> temp in result)
            {
                Console.WriteLine("Semester: {0}", temp.Key);
                Console.WriteLine("Course Taken:");
                foreach (string element in temp.Value)
                {
                    Console.WriteLine(element);
                }
            }
        }

        public string get_name_list()
        {
            int j = 1;
            string result_ = "Hasil BFS: \n\n";
            foreach (KeyValuePair<int, List<string>> entry in result)
            {
                result_ = result_ + "Semester " + entry.Key.ToString() + ":\n";
                foreach(string temp in entry.Value)
                {
                    result_ = result_ + j.ToString() + ". " + temp + "\n";
                    j++;
                }
                result_ = result_ + "\n";
            }

            return result_;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Glee.Drawing;

namespace topo
{

    public class GleeViewerSample
    {
        public static string filename = "../../../../" + "Daftar Kuliah.txt";

        public static void main()
        {
            //create a form
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();


            //create a viewer object
            Microsoft.Glee.GraphViewerGdi.GViewer viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();

            //create a graph object
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("graph");


            
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
            DFS resultDFS = new DFS(coursePlan.subjects);
            resultDFS.Print();

            Console.WriteLine();

            // print to console
            BFS resultBFS = new BFS(coursePlan.subjects);
            resultBFS.Print();
            

            
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

        public DFS(Dictionary<string, Subject> subjects)
        {
            courseTaken = new List<string>();

            Sort(subjects, subjects["dummy"]);

            courseTaken.Reverse();

            List<string> result = new List<string>();
            foreach (string element in courseTaken)
            {
                if (element != "dummy" && !result.Contains(element))
                {
                    result.Add(element);
                }
            }

            courseTaken = result;
        }

        public void Print()
        {
            Console.WriteLine("DFS");
            foreach (string element in courseTaken)
            {
                Console.WriteLine(element);
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
    }

    public class BFS
    {
        Dictionary<int, List<string>> result;

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
    }
}

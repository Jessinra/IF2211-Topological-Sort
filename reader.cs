using System;
using System.Collections.Generic;

namespace IF2211 {
    class RencanaKuliah {
        public struct Subject {
            public string name;
            public int n_of_preq;
            public bool isTaken;
            public List<string> preq_of;
        }

        static Dictionary<string, Subject> course_dict = new Dictionary<string, Subject>();

        static void Main(string[] args) {
            course_dict = getSubjects("Daftar Kuliah.txt");

            // Print Dictionary
            foreach (KeyValuePair<string, Subject> entry in course_dict) {
                Subject subject = entry.Value;
                Console.WriteLine("Mata Kuliah: {0}", subject.name);
                Console.WriteLine("N: {0}", subject.n_of_preq);
                Console.WriteLine("Taken? {0}", subject.isTaken);
                Console.Write("Prerequisite of: ");
                foreach (string preq in subject.preq_of) {
                    Console.Write(preq + " ");
                }
                Console.WriteLine("\n");
            }

            List<string> dfs_result = new List<string>();
            Subject root = course_dict["dummy"];
            dfs_course(root, dfs_result);

            dfs_result = filter_result(dfs_result);

            Console.WriteLine("output");
            foreach(string element in dfs_result){
                Console.WriteLine(element);
            }







        }

        static Dictionary<string, Subject> getSubjects(string fileName) {
            // Create dictionary with subject's name as the key
            Dictionary<string, Subject> subjects = new Dictionary<string, Subject>();

            // Create dummy to be the parent of first semester subjects
            Subject dummySubject = new Subject();
            dummySubject.name = "dummy";
            dummySubject.n_of_preq = -1;
            dummySubject.isTaken = false;
            dummySubject.preq_of = new List<string>();

            // Add dummy to dictionary
            subjects.Add("dummy", dummySubject);

            // Read file
            string[] subjectText = System.IO.File.ReadAllLines(fileName);

            // Add subject's name, subject's number of prerequisite, set isTaken to false, and create list of prerequisite
            foreach (string subjectData in subjectText) {
                string[] data = subjectData.Split(new string[] {", ", "."}, StringSplitOptions.RemoveEmptyEntries);
                Subject subject = new Subject();

                subject.name = data[0];
                subject.n_of_preq = data.Length - 1;
                subject.isTaken = false;
                subject.preq_of = new List<string>();

                subjects.Add(data[0], subject);

                if (subject.n_of_preq == 0) {
                    subjects["dummy"].preq_of.Add(subject.name);
                }
            }

            // Fill the list of prerequisite of each subject
            foreach (string subjectData in subjectText) {
                string[] data = subjectData.Split(new string[] {", ", "."}, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < data.Length; i++) {
                    subjects[data[i]].preq_of.Add(data[0]);
                }
            }

            return subjects;
        }


        static List<string> init_course_taken_list(){
            // Initialize taken course list
            List<string> course_taken = new List<string>();
            return course_taken;
        }


        static bool is_prerequisite(Subject current_subject, List<string> course_taken_list){
            // check if subject has anticedent that is not taken

            List<string> preq_of = current_subject.preq_of;
            foreach(string course in preq_of){

                // if theres courses to take, and not yet taken
                if (!(course_taken_list.Contains(course))){

                    return true;
                }    
            }
            return false;
        }

        static void dfs_course(Subject current_subject, List<string> course_taken_list){


            // if subject not taken yet
            if  (!current_subject.isTaken){

                Console.Write("checking: ");
                Console.Write(current_subject.isTaken);
                Console.WriteLine(current_subject.name);

                // take that subject
                current_subject.isTaken = true;

                // add to taken list (move into node)
                course_taken_list.Add(current_subject.name);
            
                // Select next course
                if (is_prerequisite(current_subject, course_taken_list)){

                    List<string> next_courses = current_subject.preq_of;
                    foreach(string course_name in next_courses){

                        Subject next_course = course_dict[course_name];
                        dfs_course(next_course, course_taken_list);
                    }
                }

                
                
                // add to taken list again (return from node)
                course_taken_list.Add(current_subject.name);
                Console.Write("return from: ");
                Console.Write(current_subject.isTaken);
                Console.WriteLine(current_subject.name);

                // modify 
                course_dict[current_subject.name] = current_subject;
            }
        }


        static List<string> filter_result(List<string> result){

            List<string> filtered = new List<string>();
            result.Reverse();
            foreach(string element in result){

                if (element != "dummy" && !filtered.Contains(element)){
                    filtered.Add(element);
                }
            }

            return filtered;
        }   
    }
}


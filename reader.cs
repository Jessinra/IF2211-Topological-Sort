using System;
using System.Collections.Generic;

namespace IF2211 {
    class RencanaKuliah {
        public struct Subject {
            public string name;
            public int n_of_preq;
            public bool isTaken;
            public List<string> preq;
        }

        static void Main(string[] args) {
            Dictionary<string, Subject> subjects = new Dictionary<string, Subject>();
            subjects = getSubjects("Daftar Kuliah.txt");

            // Print Dictionary
            foreach (KeyValuePair<string, Subject> entry in subjects) {
                Subject subject = entry.Value;
                Console.WriteLine("Mata Kuliah: {0}", subject.name);
                Console.WriteLine("N: {0}", subject.n_of_preq);
                Console.WriteLine("Taken? {0}", subject.isTaken);
                Console.Write("Prerequisite of: ");
                foreach (string preq in subject.preq) {
                    Console.Write(preq + " ");
                }
                Console.WriteLine("\n");
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
            dummySubject.preq = new List<string>();

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
                subject.preq = new List<string>();

                subjects.Add(data[0], subject);

                if (subject.n_of_preq == 0) {
                    subjects["dummy"].preq.Add(subject.name);
                }
            }

            // Fill the list of prerequisite of each subject
            foreach (string subjectData in subjectText) {
                string[] data = subjectData.Split(new string[] {", ", "."}, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < data.Length; i++) {
                    subjects[data[i]].preq.Add(data[0]);
                }
            }

            return subjects;
        }
    }
}



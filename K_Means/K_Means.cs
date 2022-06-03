using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace K_Means
{
    class K_Means
    {
        private static int sampleVector_length = 3, minSample_value = 0, maxSample_value = 255, iterations = 0, maxIterations = 20, threshold = 1;
        K_Means_Sample sample;          private int sampleSize = 5;
        K_Means_Classifier classifier;  private int classifierSize = 2;

        public K_Means() { Console.WriteLine("K_Means initialised without sizes. Default size is 5 Sample vectors and 2 classifier vectors."); }
        public K_Means(int sampleSize, int classifierSize, int sampleV_Length) 
        {
            this.sampleSize = sampleSize;
            this.classifierSize = classifierSize;
            sampleVector_length = sampleV_Length;
            sample = new K_Means_Sample(sampleSize, sampleVector_length, minSample_value, maxSample_value);
            classifier = new K_Means_Classifier(classifierSize, sampleVector_length, minSample_value, maxSample_value);
            
            Console.WriteLine("K_Means initialised with: SampleSize = " + sampleSize.ToString() + " || ClassifierSize = " + classifierSize.ToString() + ".");
        }

        public void Start()
        {
            Console.WriteLine(this.ToString());
            Classify();
        }
        public void Classify() 
        {
            K_Means_Vector[] copyV = new K_Means_Vector[classifier.KV.Length];
            ClearAssignedSample();
            for(int i = 0; i < classifier.KV.Length; i++)
            {
                copyV[i].Vector = classifier.KV[i].Vector;
            }
            for(int i = 0; i < sample.KV.Length; i++)
            {
                AssignSample(sample.KV[i], i);
            }
            for(int i = 0; i < classifier.KV.Length; i++)
            {
                classifier.KV[i] = RecalculateClassifier(classifier.KV[i], classifier.AssignedIndex[i]);
            }
            Console.WriteLine(this.ToString());
            if (iterations > maxIterations) { return; }
            for (int i = 0; i < classifier.KV.Length; i++)
            {
                if (EuclidianDistance(classifier.KV[i], copyV[i]) > threshold)
                {
                    iterations++;
                    Classify();
                    return;
                }

            }
        }

        protected K_Means_Vector RecalculateClassifier(K_Means_Vector vector, List<int> assigned)
        {
            K_Means_Vector copyV = vector;
            if(assigned.Count <= 0) { return vector; }
                for(int i = 0; i < vector.Vector.Length; i++)
                {
                    foreach(int index in assigned)
                    {
                        copyV.Vector[i] += sample.KV[index].Vector[i];
                    }
                copyV.Vector[i] /= assigned.Count + 1;
                }

            return copyV;
        }

        protected void AssignSample(K_Means_Vector sampleV, int sampleIndex)
        {
            float minDist, midCalc = 0; int minIndex = 0;
            minDist = EuclidianDistance(classifier.KV[0], sampleV);

            for(int i = 1; i < classifier.KV.Length; i++)
            {
                midCalc = EuclidianDistance(classifier.KV[i], sampleV);
                if(midCalc < minDist) { minDist = midCalc; minIndex = i; }
            }

            if(classifier.AssignedIndex[minIndex] != null)
            {
                if (!classifier.AssignedIndex[minIndex].Contains(sampleIndex))
                {
                    classifier.AssignedIndex[minIndex].Add(sampleIndex);
                    classifier.AssignedIndex[minIndex].Sort();
                }
            }
            else
            {
                classifier.AssignedIndex[minIndex].Add(sampleIndex);
                classifier.AssignedIndex[minIndex].Sort();
            }

        }

        protected void ClearAssignedSample()
        {
            foreach(List<int> assignedIndex in classifier.AssignedIndex)
            {
                assignedIndex.Clear();
            }
        }

        protected float EuclidianDistance(K_Means_Vector classifier, K_Means_Vector sample)
        {
            float value = 0, midcalc = 0;
            if(classifier.Vector.Length != sampleVector_length) { throw new Exception("Classifier vector length is off"); }
            if(sample.Vector.Length != sampleVector_length) { throw new Exception("Sample vector length is off"); }
            for(int i = 0; i < sampleVector_length; i++) 
            {
                midcalc= classifier.Vector[i] - sample.Vector[i];
                value += midcalc * midcalc;
            }
            value = MathF.Sqrt(value); //Console.WriteLine("Euclidian distance of "+ classifier.ToString() + " and " + sample.ToString() + "= " + value.ToString());
            return (value); 
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("We are on iteration: " + iterations.ToString());
            builder.AppendLine();
            builder.AppendLine("This K-Means has " + sampleSize.ToString() + " sample vectors and " + classifierSize.ToString() + " classifier vectors. (Vector dimension " + sampleVector_length.ToString() + ")");
            builder.AppendLine("Value Range: Min -->" + minSample_value.ToString() + "|| Max --> " + maxSample_value.ToString());
            builder.AppendLine();
            builder.AppendLine("Sample:");    builder.AppendLine(sample.ToString());
            builder.AppendLine("Classifier:");    builder.AppendLine(classifier.ToString());
            return builder.ToString();
        }

        public K_Means_Sample Sample { get { return sample; } }
        public K_Means_Classifier Classifier { get { return classifier; } }
    }

    class K_Means_Entity
    {
        protected K_Means_Vector[] kV;
        protected int vectorSize;
        public K_Means_Entity(int size, int vectorSize, int min, int max)
        {
            this.vectorSize = vectorSize;
            kV = new K_Means_Vector[size];
            for(int i = 0; i < size; i++)
            {
                kV[i] = new K_Means_Vector(vectorSize, min, max);
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Number of contained vectors = " + kV.Length.ToString() + " of the "+ vectorSize.ToString()+ " Dimension.");
            for(int i = 0; i < kV.Length; i++)
            {
                builder.AppendLine("Data of Vector " + i.ToString() + " " + kV[i].ToString());
            }
            
            return builder.ToString();
        }

        public K_Means_Vector[] KV
        {
            get { return kV; }
        }

    }

    class K_Means_Sample : K_Means_Entity 
    {
        public K_Means_Sample(int size, int vectorSize, int min, int max) : base(size, vectorSize, min, max)
        {

        }

    }

    class K_Means_Classifier : K_Means_Entity
    {
        protected List<int>[] assignedIndex;
        public K_Means_Classifier(int size, int vectorSize, int min, int max) : base(size, vectorSize, min, max)
        {
            assignedIndex = new List<int>[size];
            for(int i = 0; i < size; i++)
            {
                assignedIndex[i] = new List<int>();
            }
        }

        public List<int>[] AssignedIndex
        {
            get { return assignedIndex; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Number of contained vectors = " + kV.Length.ToString() + " of the " + vectorSize.ToString() + " Dimension.");
            for (int i = 0; i < kV.Length; i++)
            {
                builder.AppendLine("Data of Vector " + i.ToString() + " " + kV[i].ToString());
                builder.AppendLine(ToString_AssignedIndex(i));
            }
            return builder.ToString();
        }
        public string ToString_AssignedIndex(int i)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Assigned Indexes are:");
            if(assignedIndex[i] == null)
            {
                builder.Append("    null");
                return builder.ToString();
            }
            foreach(int index in assignedIndex[i])
            {
                builder.Append("    " + index.ToString());
            }
            return builder.ToString();
        } 
    }
    struct K_Means_Vector
    {
        float[] vector;

        public K_Means_Vector(int length)
        {
            vector = new float[length];
        }

        public K_Means_Vector(int length, int min, int max)
        {
            Random random = new Random();
            vector = new float[length];
            for(int i = 0; i < length; i++)
            {
                vector[i] = random.Next(min, max);
            }
        }

        public float[] Vector
        {
            get { return vector;}
            set { CopyVector(value); }
        }

        private void CopyVector(float[] toCopy)
        {
            vector = new float[toCopy.Length];
            for(int i = 0; i < toCopy.Length; i++)
            {
                vector[i] = toCopy[i];
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Vector -->");
            foreach (float element in vector)
            {
                builder.Append("    " + element.ToString());
            }
            return builder.ToString();
        }
    }
}

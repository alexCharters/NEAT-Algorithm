using System;
using System.Collections.Generic;
using NEAT;

namespace MAIN
{
	class Program
	{
		static void Main(string[] args)
		{
			List<double> inputs = GetInputs();


			List<string> outputs = GetOutputs();

			//NeuralNetwork NN = new NeuralNetwork(inputs, outputs, new Dictionary<int, Tuple<int, int>>());
			//for (int i = 0; i < 1000; i++)
			//{
			//	NN.MutateLink();
			//}

			//for (int i = 0; i < 1000; i++)
			//{
			//	NN.MutateNeuron();
			//}

			HashSet<Tuple<int, int>> innovationNumbers = new HashSet<Tuple<int, int>>();

			NeuralNetwork NN1 = new NeuralNetwork(inputs, outputs, innovationNumbers);
			NeuralNetwork NN2 = new NeuralNetwork(inputs, outputs, innovationNumbers);
			for (int i = 0; i < 15; i++)
			{
				NN1.MutateLink();
				NN2.MutateLink();
			}
			Console.WriteLine(NN1.ToString());
			Console.WriteLine(NN2.ToString());

			NN2.MutateNeuron();
			NN2.MutateNeuron();
			NN2.MutateNeuron();
			NN2.MutateNeuron();

			for (int i = 0; i < 15; i++)
			{
				NN2.MutateLink();
			}

			Console.WriteLine(NN2.ToString());
			Console.WriteLine(NN2.generateDOT());
			Console.WriteLine("Done");
			Console.ReadLine();
		}

		public static List<double> GetInputs()
		{
			return new List<double>
			{
				12,
				1,
				2,
				7,
				2,
				12,
				12,
				12,
				12,
				12
			};
		}

		public static List<string> GetOutputs()
		{
			return new List<string>
			{
				"W",
				"A",
				"S",
				"D"
			};
		}
	}
}

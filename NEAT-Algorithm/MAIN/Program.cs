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
			//for (int i = 0; i < 15; i++)
			//{
			//	NN1.MutateLink();
			//	NN2.MutateLink();
			//}

			//NN2.MutateNeuron();
			//NN2.MutateNeuron();
			//NN2.MutateNeuron();
			//NN2.MutateNeuron();

			//for (int i = 0; i < 15; i++)
			//{
			//	NN2.MutateLink();
			//}

			NN1.InitializeRandom();
			NN2.InitializeRandom();
			NN1.Fitness = 100;
			NN2.Fitness = 90;
			NeuralNetwork child = NeuralNetwork.Crossover(NN1, NN2);

			Console.WriteLine(NN1.generateDOT());
			Console.WriteLine(NN2.generateDOT());
			Console.WriteLine(child.generateDOT());
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
			};
		}

		public static List<string> GetOutputs()
		{
			return new List<string>
			{
				"Out"
			};
		}
	}
}

using System;
using System.Collections.Generic;
using NEAT;

namespace MAIN
{
	class Program
	{
		static void Main(string[] args)
		{
			List<double> inputs = new List<double>();
			inputs.Add(12);
			inputs.Add(1);
			inputs.Add(2);
			inputs.Add(7);
			inputs.Add(2);
			inputs.Add(12);
			inputs.Add(12);
			inputs.Add(12);
			inputs.Add(12);
			inputs.Add(12);


			List<string> outputs = new List<string>();
			outputs.Add("W");
			outputs.Add("A");
			outputs.Add("S");
			outputs.Add("D");

			NeuralNetwork NN = new NeuralNetwork(inputs, outputs);
			for (int i = 0; i < 1000; i++)
			{
				NN.mutateLink();
			}

			for (int i = 0; i < 1000; i++)
			{
				NN.mutateNeuron();
			}
			Console.WriteLine("Done");
			Console.ReadLine();
		}
	}
}

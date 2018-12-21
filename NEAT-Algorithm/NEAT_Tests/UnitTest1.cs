using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NEAT;

namespace NEAT_Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			List<double> inputs = getInputs();


			List<string> outputs = getOutputs();

			Population pop = new Population(inputs, outputs, 200);
		}

		[TestMethod]
		public void InnovationNumberTest()
		{
			Dictionary<int, Tuple<int, int>> innovationNumbers = new Dictionary<int, Tuple<int, int>>();
			List<double> inputs = getInputs();
			List<string> outputs = getOutputs();

			NeuralNetwork NN1 = new NeuralNetwork(inputs, outputs, innovationNumbers);
		}



		public List<double> getInputs()
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

		public List<string> getOutputs()
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

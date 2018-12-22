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
			List<double> inputs = GetInputs();


			List<string> outputs = GetOutputs();

			Population pop = new Population(inputs, outputs, 200);
		}

		[TestMethod]
		public void InnovationNumberTest()
		{
			HashSet<Tuple<int, int>> innovationNumbers = new HashSet<Tuple<int, int>>();
			List<double> inputs = GetInputs();
			List<string> outputs = GetOutputs();

			NeuralNetwork NN1 = new NeuralNetwork(inputs, outputs, innovationNumbers);
			NeuralNetwork NN2 = new NeuralNetwork(inputs, outputs, innovationNumbers);
			for (int i = 0; i < 15; i++)
			{
				NN1.MutateLink();
				NN2.MutateLink();
			}
			Console.WriteLine(NN1.ToString());
			Console.WriteLine(NN2.ToString());
		}

		[TestMethod]
		public void TupleHashsetContainsTest()
		{
			HashSet<Tuple<int, int>> hash = new HashSet<Tuple<int, int>>
			{
				new Tuple<int, int>(1, 7)
			};
			Assert.IsTrue(hash.Contains(new Tuple<int, int>(1, 7)));
		}

		[TestMethod]
		public void TupleHashcodeTest()
		{
			Tuple<int, int> t1 = new Tuple<int, int>(2,9);
			Tuple<int, int> t2 = new Tuple<int, int>(2, 9);

			Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
		}

		public List<double> GetInputs()
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

		public List<string> GetOutputs()
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

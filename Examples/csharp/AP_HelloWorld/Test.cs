using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP_HelloWorld
{
	public interface ITester
	{
		/// <summary>
		/// says hello
		/// </summary>
		/// <param name="msg">the hello msg</param>
		void HelloFromTester(string msg);
	}
	class Test : ITester
	{
		/// <summary>
		/// 
		/// </summary>
		public int _TestInt = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="P1">my p1</param>
		/// <returns></returns>
		public bool Func1(int P1)
		{
			return false;
		}

		public void Func2()
		{
			Func1(5);
		}

		void ITester.HelloFromTester(string msg)
		{
			throw new NotImplementedException();
		}
	}

	public class C2
	{
		public void hello()
		{
			Test t = new Test();
			ITester it = t as ITester;
			 

		}
	}
}

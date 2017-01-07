﻿using System;
using CoreGraphics;

namespace iSign.Helpers
{
	public static class CGRectHelpers
	{
		public static CGRect CGRectNull ()
		{
			return new CGRect (nfloat.PositiveInfinity, nfloat.PositiveInfinity, 0, 0);
		}
	}
}

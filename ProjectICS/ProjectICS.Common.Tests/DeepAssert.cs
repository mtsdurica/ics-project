﻿using KellermanSoftware.CompareNetObjects;
using Xunit.Sdk;

namespace ProjectICS.Common.Tests;

public static class DeepAssert
{
    public static void Equal<T>(T? expected, T? actual, params string[] propertiesToIgnore)
    {
        CompareLogic compareLogic = new()
        {
            Config =
            {
                MembersToIgnore = propertiesToIgnore.ToList(),
                IgnoreCollectionOrder = true,
                IgnoreObjectTypes = true,
                CompareStaticProperties = false,
                CompareStaticFields = false
            }
        };

        ComparisonResult comparisonResult = compareLogic.Compare(expected!, actual!);
        if (!comparisonResult.AreEqual)
        {
            throw new AssertActualExpectedException(expected!, actual!, comparisonResult.DifferencesString);
        }
    }
}

﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.Internal
{
    internal static class Error
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowArgumentNullException<T>(T value, string paramName)
          where T : class
        {
            if (value == null) ThrowArgumentNullExceptionCore(paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowArgumentNullExceptionCore(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowWhenContinuationIsAlreadyRegistered<T>(T continuationField)
          where T : class
        {
            if (continuationField != null) ThrowInvalidOperationExceptionCore("continuation is already registered.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowInvalidOperationExceptionCore(string message)
        {
            throw new InvalidOperationException(message);
        }
    }
}


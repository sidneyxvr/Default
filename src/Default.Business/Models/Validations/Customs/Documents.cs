﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Default.Business.Models.Validations.Customs
{
    public class Documents
    {
        public class CpfValidation
        {
            public const int CpfSize = 11;

            public static bool Validate(string cpf)
            {
                var cpfNumbers = Utils.OnlyNumbers(cpf);

                if (!ValidSize(cpfNumbers)) return false;
                return !HasRepeatedDigits(cpfNumbers) && HasValidDigits(cpfNumbers);
            }

            private static bool ValidSize(string value)
            {
                return value.Length == CpfSize;
            }

            private static bool HasRepeatedDigits(string value)
            {
                string[] invalidNumbers =
                {
                    "00000000000",
                    "11111111111",
                    "22222222222",
                    "33333333333",
                    "44444444444",
                    "55555555555",
                    "66666666666",
                    "77777777777",
                    "88888888888",
                    "99999999999"
                };
                return invalidNumbers.Contains(value);
            }

            private static bool HasValidDigits(string value)
            {
                var number = value.Substring(0, CpfSize - 2);
                var verifiyngDigit = new DigitoVerificador(number)
                    .withMultipliersUpTo(2, 11)
                    .Replacing("0", 10, 11);
                var firstDigit = verifiyngDigit.CalculaDigito();
                verifiyngDigit.AddDigito(firstDigit);
                var secondDigit = verifiyngDigit.CalculaDigito();

                return string.Concat(firstDigit, secondDigit) == value.Substring(CpfSize - 2, 2);
            }
        }

        public class CnpjValidation
        {
            public const int CnpjSize = 14;

            public static bool Validate(string cpnj)
            {
                var cnpjNumeros = Utils.OnlyNumbers(cpnj);

                if (!HasValidSize(cnpjNumeros)) return false;
                return !HasRepeatedDigits(cnpjNumeros) && HasValidDigits(cnpjNumeros);
            }

            private static bool HasValidSize(string value)
            {
                return value.Length == CnpjSize;
            }

            private static bool HasRepeatedDigits(string value)
            {
                string[] invalidNumbers =
                {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };
                return invalidNumbers.Contains(value);
            }

            private static bool HasValidDigits(string value)
            {
                var number = value.Substring(0, CnpjSize - 2);

                var verifiyngDigit = new DigitoVerificador(number)
                    .withMultipliersUpTo(2, 9)
                    .Replacing("0", 10, 11);
                var firstDigit = verifiyngDigit.CalculaDigito();
                verifiyngDigit.AddDigito(firstDigit);
                var secondDigit = verifiyngDigit.CalculaDigito();

                return string.Concat(firstDigit, secondDigit) == value.Substring(CnpjSize - 2, 2);
            }
        }

        public class DigitoVerificador
        {
            private string _number;
            private const int Module = 11;
            private readonly List<int> _multipliers = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
            private readonly IDictionary<int, string> _replacements = new Dictionary<int, string>();
            private bool _complementaryModule = true;

            public DigitoVerificador(string number)
            {
                _number = number;
            }

            public DigitoVerificador withMultipliersUpTo(int firstMultiplier, int lastMultiplier)
            {
                _multipliers.Clear();
                for (var i = firstMultiplier; i <= lastMultiplier; i++)
                    _multipliers.Add(i);

                return this;
            }

            public DigitoVerificador Replacing(string substitute, params int[] digits)
            {
                foreach (var i in digits)
                {
                    _replacements[i] = substitute;
                }
                return this;
            }

            public void AddDigito(string digit)
            {
                _number = string.Concat(_number, digit);
            }

            public string CalculaDigito()
            {
                return !(_number.Length > 0) ? "" : GetDigitSum();
            }

            private string GetDigitSum()
            {
                var soma = 0;
                for (int i = _number.Length - 1, m = 0; i >= 0; i--)
                {
                    var produto = (int)char.GetNumericValue(_number[i]) * _multipliers[m];
                    soma += produto;

                    if (++m >= _multipliers.Count) m = 0;
                }

                var mod = (soma % Module);
                var result = _complementaryModule ? Module - mod : mod;

                return _replacements.ContainsKey(result) ? _replacements[result] : result.ToString();
            }
        }

        public class Utils
        {
            public static string OnlyNumbers(string value)
            {
                var onlyNumber = "";
                foreach (var s in value)
                {
                    if (char.IsDigit(s))
                    {
                        onlyNumber += s;
                    }
                }
                return onlyNumber.Trim();
            }
        }
    }
}

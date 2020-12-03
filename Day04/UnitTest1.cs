using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day04
{
    public class Tests
    {

        [Test]
        public void TestPassportsParserCorrectlySplitsDataIntoPassports()
        {
            var data = 
                @"hgt:176cm
iyr:2013
hcl:#fffffd ecl:amb
byr:2000
eyr:2034
cid:89 pid:934693255

hcl:#b5c3db ecl:grn hgt:155cm pid:#baec97 iyr:2017
byr: 1939
eyr: 2020

pid:526669252 eyr:1972
hgt:152cm ecl:dne byr:1960 hcl:z iyr:2023";

            var passports = PassportsParser.Parse(data);

            Assert.That(passports.Count, Is.EqualTo(3));
        }

        [Test]
        public void PassportHasThreeFieldsOnThreeLines()
        {
            var data = @"pid:526669252 
eyr:1972
hgt:152cm";

            var passport = new Passport(data);

            Assert.That(passport.NumOfFields, Is.EqualTo(3));
        }

        [Test]
        public void PassportHasThreeFieldsOnOneLine()
        {
            var data = @"pid:526669252 eyr:1972 hgt:152cm";

            var passport = new Passport(data);

            Assert.That(passport.NumOfFields, Is.EqualTo(3));

        }

        [Test]
        public void PassportIsValidWhenItHasAllEightFields()
        {
            var data = @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd byr:1937 iyr:2017 cid:147 hgt:183cm";

            var passport = new Passport(data);

            Assert.That(passport.ContainsRequiredFields(), Is.True);
        }

        [Test]
        public void PassportIsValidWhenOnlyMissingCidField()
        {
            var data = @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd byr:1937 iyr:2017 hgt:183cm";

            var passport = new Passport(data);

            Assert.That(passport.ContainsRequiredFields(), Is.True);
        }

        [Test]
        public void PassportGetFieldValue()
        {
            var birthYear = "1937";
            var eyeColor = "gry";
            var pid = "860033327";
            var expirationYear = "2020";
            var hairColor = "#fffffd";
            var issueYear = "2017";
            var cid = "147";
            var height = "183cm";

            var data = $"ecl:{eyeColor} pid:{pid} eyr:{expirationYear} hcl:{hairColor} byr:{birthYear} iyr:{issueYear} cid:{cid} hgt:{height}";

            var passport = new Passport(data);

            Assert.That(passport.GetFieldValue(Passport.Field.BYR), Is.EqualTo(birthYear));
            Assert.That(passport.GetFieldValue(Passport.Field.ECL), Is.EqualTo(eyeColor));
            Assert.That(passport.GetFieldValue(Passport.Field.PID), Is.EqualTo(pid));
            Assert.That(passport.GetFieldValue(Passport.Field.EYR), Is.EqualTo(expirationYear));
            Assert.That(passport.GetFieldValue(Passport.Field.HCL), Is.EqualTo(hairColor));
            Assert.That(passport.GetFieldValue(Passport.Field.IYR), Is.EqualTo(issueYear));
            Assert.That(passport.GetFieldValue(Passport.Field.CID), Is.EqualTo(cid));
            Assert.That(passport.GetFieldValue(Passport.Field.HGT), Is.EqualTo(height));
        }

        [Test]
        [TestCase("123456789", true)]
        [TestCase("12345678", false)]
        [TestCase("1234567890", false)]
        [TestCase("000123456", true)]
        public void TestPassportIdValidator(string pid, bool isValid)
        {
            var validator = new PassportIdValidator();
            Assert.That(validator.IsValid(pid), Is.EqualTo(isValid));
        }

        [Test]
        [TestCase("amb", true)]
        [TestCase("blu", true)] 
        [TestCase("brn", true)] 
        [TestCase("gry", true)] 
        [TestCase("grn", true)] 
        [TestCase("hzl", true)] 
        [TestCase("oth", true)]
        [TestCase("xxx", false)]
        public void TestEyeColorValidator(string eyeColor, bool isValid)
        {
            var validator = new EyeColorValidator();
            Assert.That(validator.IsValid(eyeColor), Is.EqualTo(isValid));
        }

        [Test]
        [TestCase("#123456", true)]
        [TestCase("#987654", true)]
        [TestCase("123456", false)]
        [TestCase("0123456", false)]
        [TestCase("#abcdef", true)]
        [TestCase("#abcdex", false)]
        public void TestHairColorValidator(string hairColor, bool isValid)
        {
            var validator = new HairColourValidator();
            Assert.That(validator.IsValid(hairColor), Is.EqualTo(isValid));
        }

        [Test]
        [TestCase("150cm", true)]
        [TestCase("180cm", true)]
        [TestCase("193cm", true)]
        [TestCase("149cm", false)]
        [TestCase("194cm", false)]
        [TestCase("59in", true)]
        [TestCase("76in", true)]
        [TestCase("60in", true)]
        [TestCase("58in", false)]
        [TestCase("77in", false)]
        [TestCase("165", false)]
        [TestCase("65", false)]
        [TestCase("in", false)]
        [TestCase("cm", false)]
        [TestCase("abc190", false)]
        [TestCase("in60", false)]
        [TestCase("cm180", false)]
        public void TestHeightValidator(string height, bool isValid)
        {
            var validator = new HeightValidator();
            Assert.That(validator.IsValid(height), Is.EqualTo(isValid));
        }

        [Test]
        [TestCase("2020", true)]
        [TestCase("2030", true)]
        [TestCase("2019", false)]
        [TestCase("2031", false)]
        [TestCase("1", false)]
        [TestCase("20200", false)]
        public void TestYearValidator(string expYear, bool isValid)
        {
            var validator = new YearValidator(Passport.Field.EYR, 2020, 2030);
            Assert.That(validator.IsValid(expYear), Is.EqualTo(isValid));
        }

        [Test]
        public void Puzzle01()
        {
            var data = File.ReadAllText("data.txt");
            var passports = PassportsParser.Parse(data);
            
            var valid = passports.Where(p => p.ContainsRequiredFields());
            System.Console.WriteLine($"There are {valid.Count()} valid passports");
            
            Assert.That(valid.Count, Is.EqualTo(230));
        }

        [Test]
        public void Puzzle02()
        {
            var data = File.ReadAllText("data.txt");
            var passports = PassportsParser.Parse(data);

            var passportValidator = new PassportValidator(new List<FieldValidator>
            {
                new YearValidator(Passport.Field.IYR, 2010, 2020),
                new YearValidator(Passport.Field.BYR, 1920, 2002),
                new YearValidator(Passport.Field.EYR, 2020, 2030),
                new HeightValidator(),
                new HairColourValidator(),
                new EyeColorValidator(),
                new PassportIdValidator()
            });

            var validPassports = passports.Where(passportValidator.IsValid);

            System.Console.WriteLine($"There are {validPassports} valid passports");

            Assert.That(validPassports.Count(), Is.EqualTo(156));
        }
    }

    class PassportsParser
    {
        public static List<Passport> Parse(string data)
        {
            return data.Split("\n\n").Select(l => new Passport(l)).ToList();
        }
    }

    class Passport
    {
        private List<string> _data = new List<string>();

        private readonly List<Field> RequiredFields = new List<Field>
        {
            Field.BYR,
            Field.IYR,
            Field.EYR,
            Field.HGT,
            Field.HCL,
            Field.ECL,
            Field.PID
        };

        public enum Field
        {
            BYR,
            IYR,
            EYR,
            HGT,
            HCL,
            ECL,
            PID,
            CID
        }

        public Passport(string data)
        {
            _data = data.Split(null).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        }

        public int NumOfFields => _data.Count;

        public bool ContainsRequiredFields()
        {
            foreach (var key in RequiredFields)
            {
                if (_data.Count(s => s.Contains(key.ToString().ToLower())) != 1)
                {
                    return false;
                }
            }
            return true; 
        }

        public string GetFieldValue(Field field)
        {
            var fieldDetails = _data.Find(d => d.Contains(field.ToString().ToLower()));
            return fieldDetails.Substring(fieldDetails.IndexOf(':') + 1);
        }
    }

    class PassportValidator
    {
        List<FieldValidator> _fieldValidators;
        public PassportValidator(List<FieldValidator> fieldValidators)
        {
            _fieldValidators = fieldValidators;
        }

        public bool IsValid(Passport passport)
        {
            if (!passport.ContainsRequiredFields())
            {
                return false;
            }

            foreach (var validator in _fieldValidators)
            {
                if (!validator.Validate(passport))
                {
                    return false;
                };
            }

            return true;
        }
    }

    interface FieldValidator
    {
        public Passport.Field Field();

        public bool Validate(Passport passport)
        {
            var field = passport.GetFieldValue(Field());
            return IsValid(field);
        }

        public bool IsValid(string fieldValue);
    }

    internal class PassportIdValidator : FieldValidator
    {
        public Passport.Field Field()
        {
            return Passport.Field.PID;
        }

        public bool IsValid(string fieldValue)
        {
            var isNineCharsLong = fieldValue.Length == 9;
            var result = 0;
            var isANumber = int.TryParse(fieldValue, out result);
            return isNineCharsLong && isANumber;
        }
    }

    internal class EyeColorValidator : FieldValidator
    {
        public Passport.Field Field()
        {
            return Passport.Field.ECL;
        }

        public bool IsValid(string fieldValue)
        {
            var validOptions = new List<string>
            {
                "amb",
                "blu",
                "brn",
                "gry",
                "grn",
                "hzl",
                "oth"
            };

            return validOptions.Contains(fieldValue.ToLower());
        }
    }

    internal class HairColourValidator : FieldValidator
    {
        public Passport.Field Field()
        {
            return Passport.Field.HCL;
        }

        public bool IsValid(string fieldValue)
        {
            var containsSevenChars = fieldValue.Length == 7;
            var firstCharIsHash = fieldValue[0] == '#';

            var validChars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            foreach (var c in fieldValue.Substring(1))
            {
                if (!validChars.Contains(c)) return false;
            }

            return containsSevenChars && firstCharIsHash;
        }
    }

    internal class HeightValidator : FieldValidator
    {
        public Passport.Field Field()
        {
            return Passport.Field.HGT;
        }

        public bool IsValid(string fieldValue)
        {
            var units = fieldValue switch
            {
                string s when s.ToLower().Substring(s.Length - 2) == "cm" => Units.CM,
                string s when s.ToLower().Substring(s.Length - 2) == "in" => Units.IN,
                _ => Units.Unknown
            };

            if (units == Units.Unknown) return false;

            int value;
            var containsIntHeight = int.TryParse(fieldValue.Substring(0, fieldValue.Length - 2), out value);

            if (!containsIntHeight) return false;

            return units == Units.CM ? ValidCm(value) : ValidIn(value);
        }

        private bool ValidCm(int value)
        {
            return value >= 150 && value <= 193;
        }

        private bool ValidIn(int value)
        {
            return value >= 59 && value <= 76;
        }

        internal enum Units
        {
            IN,
            CM,
            Unknown
        }
    }

    internal class YearValidator : FieldValidator
    {
        private readonly int _lowest;
        private readonly int _highest;
        private readonly Passport.Field _field;

        public Passport.Field Field()
        {
            return _field;
        }

        public YearValidator(Passport.Field field, int lowest, int highest)
        {
            _field = field;
            _lowest = lowest;
            _highest = highest;
        }

        public bool IsValid(string fieldValue)
        {
            int year;
            var result = int.TryParse(fieldValue, out year);
            return result && (year >= _lowest && year <= _highest);
        }
    }
}
﻿function myanmarNumbers(str, toLanguage) {
    str += "";
    toLanguage = (toLanguage || "en").toLowerCase();

    var replaceNumbers = function (txt) {
        var numbers = {
            // Myanmar and Shan numbers
            "๐": 0, // Thai zero
            "ဝ": 0, // Myanmar letter "wa" sometimes used as zero
            "၀": 0,
            "၁": 1,
            "၂": 2,
            "၃": 3,
            "၄": 4,
            "၅": 5,
            "၆": 6,
            "၇": 7,
            "၈": 8,
            "၉": 9,
            "႐": 0,
            "႑": 1,
            "႒": 2,
            "႓": 3,
            "႔": 4,
            "႕": 5,
            "႖": 6,
            "႗": 7,
            "႘": 8,
            "႙": 9,


        };

        var keys = Object.keys(numbers);
        if (toLanguage === "my") {
            // Myanmar
            for (var n = 2; n <= 11; n++) {
                var re = new RegExp(numbers[keys[n]] + "", "g");
                txt = txt.replace(re, keys[n]);
            }
        } else if (toLanguage === "shan") {
            // Shan numerals - convert any Myanmar numbers to international first
            var txt = myanmarNumbers(txt) + "";
            for (var n = 12; n < keys.length; n++) {
                var re = new RegExp(numbers[keys[n]] + "", "g");
                txt = txt.replace(re, keys[n]);
            }
        } else {
            for (var n = 0; n < keys.length; n++) {
                // default
                if (n === 1) {
                    txt = txt.replace(/([၁၂၃၄၅၆၇၈၉])ဝ/g, '$10');
                    txt = txt.replace(/ဝ([၁၂၃၄၅၆၇၈၉])/g, '0$1');
                    while (txt.match(/ဝ(\d)/)) {
                        txt = txt.replace(/ဝ(\d)/g, '0$1');
                    }
                    while (txt.match(/(\d)ဝ/)) {
                        txt = txt.replace(/(\d)ဝ/g, '$10');
                    }
                } else {
                    var re = new RegExp(keys[n], "g");
                    txt = txt.replace(re, numbers[keys[n]]);
                }
            }
        }
        return txt;
    };

    var numerals = replaceNumbers(str);
    return 1 * numerals;
}

if (typeof module !== "undefined") {
    module.exports = myanmarNumbers;
}
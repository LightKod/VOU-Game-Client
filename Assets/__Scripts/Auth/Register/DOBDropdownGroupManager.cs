using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace VOU
{
    public class DOBDropdownGroupManager : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dayDropdown;
        [SerializeField] private TMP_Dropdown monthDropdown;
        [SerializeField] private TMP_Dropdown yearDropdown;

        // Start is called before the first frame update
        void Start()
        {
            PopulateDayDropdown();
            PopulateMonthDropdown();
            PopulateYearDropdown();

            monthDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });
            yearDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });
        }

        private void UpdateDayDropdown()
        {
            PopulateDayDropdown(dayDropdown.value + 1);
        }

        private void PopulateYearDropdown()
        {
            yearDropdown.ClearOptions();
            List<string> years = new List<string>();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 100; i <= currentYear; i++)
            {
                years.Add(i.ToString());
            }
            yearDropdown.AddOptions(years);
        }

        private void PopulateMonthDropdown()
        {
            monthDropdown.ClearOptions();
            List<string> months = new List<string>
            { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            monthDropdown.AddOptions(months);
        }

        private void PopulateDayDropdown(int day = -1)
        {
            dayDropdown.ClearOptions();
            List<string> days = new List<string>();

            int daysInMonth = GetDaysInMonth();
            for (int i = 1; i <= daysInMonth; i++)
            {
                days.Add(i.ToString());
            }
            dayDropdown.AddOptions(days);

            if (day == -1)
            {
                dayDropdown.value = 0;
            }
            else if (day <= daysInMonth)
            {
                dayDropdown.value = day - 1;
            }
            else
            {
                dayDropdown.value = days.Count - 1;
            }
        }

        private int GetDaysInMonth()
        {
            int monthIndex = monthDropdown.value + 1;
            int year = int.Parse(yearDropdown.options[yearDropdown.value].text);

            return DateTime.DaysInMonth(year, monthIndex);
        }
    }
}

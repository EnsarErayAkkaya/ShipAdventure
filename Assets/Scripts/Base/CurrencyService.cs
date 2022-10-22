using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Services
{
    [System.Serializable]
    public enum Currency
    {
        Coin
    }
    [Serializable]
    public class CurrencyData
    {
        public Currency currency;
        public int amount;
    }

    public enum Operation
    {
        Add, Substact
    }
    public class CurrencyService : MonoBehaviour
    {
        private SaveService saveService;

        public delegate void OnCurrencyChanged(Currency currency, int amount);
        public OnCurrencyChanged onCurrencyChanged;

        public List<CurrencyData> Currencies => saveService.saveData.currencyDatas;

        private void Start()
        {
            saveService = FindObjectOfType<SaveService>();
        }

        public CurrencyData GetCurrency(Currency _currency)
        {
            CurrencyData cd = Currencies.Find(s => s.currency == _currency);
            if(cd == null)
            {
                cd = new CurrencyData()
                {
                    currency = _currency,
                    amount = 0
                };
                saveService.saveData.currencyDatas.Add(cd);
                saveService.SaveGame();
            }
            return cd;
        }

        public void ModifyCurrency(Currency _currency, int modify)
        {
            CurrencyData cd = Currencies.Find(s => s.currency == _currency);
            if (cd != null)
            {
                cd.amount += modify;
                cd.amount = Mathf.Clamp(cd.amount, 0, int.MaxValue);
            }
            else
            {
                cd = new CurrencyData()
                {
                    currency = _currency,
                    amount = 0
                };
                saveService.saveData.currencyDatas.Add(cd);
            }
            onCurrencyChanged?.Invoke(_currency, cd.amount);
            saveService.SaveGame();
        }

        public void ModifyCurrency(CurrencyData currencyData, Operation operation = Operation.Add)
        {
            CurrencyData cd = Currencies.Find(s => s.currency == currencyData.currency);
            if (cd != null)
            {
                if (operation == Operation.Add)
                    cd.amount += currencyData.amount;
                else
                    cd.amount -= currencyData.amount;

                cd.amount = Mathf.Clamp(cd.amount, 0, int.MaxValue);
            }
            else
            {
                cd = new CurrencyData()
                {
                    currency = currencyData.currency,
                    amount = 0
                };
                saveService.saveData.currencyDatas.Add(cd);
            }
            onCurrencyChanged?.Invoke(currencyData.currency, cd.amount);
            saveService.SaveGame();
        }

        public void ModifyCurrency(List<CurrencyData> currencyDatas, Operation operation = Operation.Add)
        {
            foreach (var currency in currencyDatas)
            {
                CurrencyData cd = Currencies.Find(s => s.currency == currency.currency);
                if (cd != null)
                {
                    if(operation == Operation.Add)
                        cd.amount += currency.amount;
                    else
                        cd.amount -= currency.amount;

                    cd.amount = Mathf.Clamp(cd.amount, 0, int.MaxValue);
                }
                else
                {
                    cd = new CurrencyData()
                    {
                        currency = currency.currency,
                        amount = 0
                    };
                    saveService.saveData.currencyDatas.Add(cd);
                }
                onCurrencyChanged?.Invoke(cd.currency, cd.amount);
            }
            saveService.SaveGame();
        }
    }
}
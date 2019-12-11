using Financial_Portal.Enums;
using Financial_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Extensions
{
    public static class TransactionExtension
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void UpdateBalances(this Transaction transaction)
        {
            UpdateBankAccount(transaction);
            UpdateBucket(transaction);
            UpdateBucketItem(transaction);
        }

        private static void UpdateBankAccount(Transaction transaction)
        {
            var bank = db.Accounts.Find(transaction.AccountId);
            if (transaction.TransactionType == TransactionType.Deposit)
            {
                bank.CurrentBal += transaction.Amount;
            }
            else
            {
                bank.CurrentBal -= transaction.Amount;
            }
            db.SaveChanges();
        }

        private static void UpdateBucket(Transaction transaction)
        {
            if (transaction.TransactionType == TransactionType.Deposit ||
                transaction.BucketItemId == null) { return; }
            var bucketId = db.BucketItems.Find(transaction.BucketItemId).BucketId;
            var bucket = db.Buckets.Find(bucketId);
            bucket.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }

        private static void UpdateBucketItem(Transaction transaction)
        {
            if (transaction.TransactionType == TransactionType.Deposit ||
                transaction.BucketItemId == null) { return; }
            var bucketItem = db.BucketItems.Find(transaction.BucketItemId);
            bucketItem.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }
    }
}
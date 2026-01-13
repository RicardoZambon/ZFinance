using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using ZFinance.Core.Entities.Security;

namespace ZFinance.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for user-related operations.
    /// </summary>
    public static class UsersExtensions
    {
        #region Constants
        private const int HASH_SIZE = 256 / 8;
        private const int ITERATION_COUNT = 10_000;
        private const KeyDerivationPrf PRF = KeyDerivationPrf.HMACSHA256;
        private const int SALT_SIZE = 128 / 8;
        #endregion

        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <summary>
        /// Hashes the provided password and updates the user's PasswordHash property.
        /// </summary>
        /// <param name="user">The user instance.</param>
        /// <param name="newPassword">The new password to hash.</param>
        /// <exception cref="ArgumentNullException">Thrown when the new password is <c>null</c> or an empty string.</exception>
        public static void HashStringAndUpdatePassword(this Users user, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            // Generate a new salt.
            byte[] salt = new byte[SALT_SIZE];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            // Hash the password with the salt.
            byte[] hash = GetHash(newPassword, salt);

            // Combine the salt and hash into a single byte array.
            byte[] hashBytes = new byte[SALT_SIZE + HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

            user.PasswordHash = Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies if the provided password matches the stored hashed password.
        /// </summary>
        /// <param name="user">The user instance.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>Returns <c>true</c> if the password matches the user password; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the password is <c>null</c> or an empty string.</exception>
        public static bool VerifyHashedPassword(this Users user, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (user.PasswordHash is null)
            {
                return false;
            }

            byte[] numArray = Convert.FromBase64String(user.PasswordHash);
            if (numArray.Length < 1)
            {
                return false;
            }

            // Extract the salt from the stored PasswordHash.
            byte[] salt = new byte[SALT_SIZE];
            Buffer.BlockCopy(numArray, 0, salt, 0, SALT_SIZE);

            // Extract the original hash from the stored PasswordHash.
            byte[] originalHash = new byte[HASH_SIZE];
            Buffer.BlockCopy(numArray, SALT_SIZE, originalHash, 0, HASH_SIZE);

            // Hash the provided password with the extracted salt.
            byte[] newHash = GetHash(password, salt);

            if (CryptographicOperations.FixedTimeEquals(originalHash, newHash))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Private methods
        private static byte[] GetHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(password, salt, PRF, ITERATION_COUNT, HASH_SIZE);
        }
        #endregion
    }
}
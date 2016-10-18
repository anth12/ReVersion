using FluentAssertions;
using ReVersion.Utilities.Helpers;
using Xunit;

namespace ReVersion.Tests.Encryption
{
    public class EncryptionTests
    {
        [Fact]
        public void EncryptDecrypt()
        {
            // Arrange
            var subject = new EncryptionHelper();
            var originalString = "Testing123!£$";

            // Act
            var encryptedString1 = subject.Encrypt(originalString);
            var encryptedString2 = subject.Encrypt(originalString);
            var decryptedString1 = subject.Decrypt(encryptedString1);
            var decryptedString2 = subject.Decrypt(encryptedString2);

            // Assert
            originalString.Should().Be(decryptedString1, "Decrypted string should match original string");
            originalString.Should().Be(decryptedString2, "Decrypted string should match original string");
            originalString.Should().Be(encryptedString1, "Encrypted string should not match original string");
            encryptedString1.Should().Be(encryptedString2, "String should never be encrypted the same twice");
        }
    }
}

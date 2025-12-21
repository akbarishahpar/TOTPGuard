**TOTPGuard**

**Securely share Time-based One-Time Passwords (TOTP) without revealing your secret keys.**

**OverviewTOTPGuard** is an open-source security utility designed to solve the "Shared Secret" dilemma in team operations. In standard 2FA implementations (RFC 6238), delegating access to an account requires sharing the underlying private seed (Secret Key). Once shared, this key cannot be revoked without rotating it on the service provider side—a tedious and disruptive process. TOTPGuard acts as a **Trusted Proxy**. It securely holds your secret keys and generates temporary, time-bound share links. These links allow a recipient to view the *current* live 6-digit OTP code for a specified duration, but **never** reveal the underlying cryptographic seed. **🚀 Key Features**

**Secret Isolation:** The raw TOTP seed (e.g., JBSWY...) is encrypted at rest using AES-256 and never transmitted to the client.

**Ephemeral Access:** Create links that expire automatically after a set duration (e.g., 1 hour, 24 hours).

**Live Updates:** The viewing interface auto-refreshes the OTP every 30 seconds, ensuring the recipient always has a valid code.

**Instant Revocation:** Invalidate a share link immediately if you suspect unauthorized access, without needing to rotate the root credentials on the external service.



**Usage Workflow**

**Encode Secret:** Any user can encode TOTP Secret (e.g., "Company Twitter") and its metadata (Including TOTP Step and Expiration in minutes) and obtain a hexadecimal URL.

**Distribute:** The URL can be sent to the contractor/employee via chat/email.

**Access:** Contractor opens the URL. They see the 6-digit code. They use it to log in.

**Expiration:** After expiration of the URL, the link returns 404 Not Found. The contractor's access is effectively revoked.



**Contributing**

Contributions are welcome. Feel free for submitting pull requests.



**License**

This project is licensed under the MIT License.

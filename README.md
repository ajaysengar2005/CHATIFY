# **Chatify - Realtime Chat Application**

Chatify is a **Messenger-like chat application** built using **C# (.NET Framework) and Firebase**. It features **email authentication, real-time messaging, and a user-friendly UI**.

## 🚀 **Features**
- 🔑 **User Authentication** (Firebase Email & Password Login)
- 💬 **Real-time Messaging** (Firebase Realtime Database)
- 📋 **Contact List** (Displays all registered users, except the logged-in user)
- 📩 **Send & Receive Messages Instantly**
- 🎨 **Modern UI** (Messenger-style chat window)

## 📌 **Technologies Used**
- **C# .NET Framework** (Windows Forms for UI)
- **Firebase Authentication** (Email/Password Login)
- **Firebase Realtime Database** (Storing Users & Messages)
- **FireSharp** (Firebase C# Library)

---

## ⚙️ **Setup & Installation**
### 🔹 **1. Clone the Repository**
```bash
git clone https://github.com/yourusername/Chatify.git
cd Chatify
```

### 🔹 **2. Configure Firebase**
1. Create a Firebase project at **[Firebase Console](https://console.firebase.google.com/)**.
2. **Enable Authentication** (Email/Password Sign-in Method).
3. **Set up Firebase Realtime Database** (in **Test Mode** initially).
4. Copy your **Firebase API Key & Database URL** and update it in the project files:
   - `FormRegister.cs`
   - `FormLogin.cs`
   - `FormChatWindow.cs`

Example Firebase Configuration:
```csharp
IFirebaseConfig config = new FirebaseConfig
{
    AuthSecret = "YOUR_FIREBASE_DATABASE_SECRET",
    BasePath = "https://your-database.firebaseio.com/"
};
```

### 🔹 **3. Install Dependencies**
1. Open the project in **Visual Studio**.
2. Install required **NuGet packages**:
   ```bash
   Install-Package FireSharp
   Install-Package FirebaseAuthentication.net
   ```

### 🔹 **4. Run the Project**
1. Build and run the project in **Visual Studio** (`Ctrl + F5`).
2. Register a new user, log in, and start chatting!

---

## 🖥️ **Project Structure**
```
📁 Chatify
│── 📂 Forms
│   ├── FormRegister.cs      # User Registration
│   ├── FormLogin.cs         # User Login
│   ├── FormChatWindow.cs    # Chat Window
│── 📂 Models
│   ├── User.cs              # User Model
│   ├── ChatMessage.cs       # Message Model
│── README.md                # Project Documentation
│── Chatify.sln              # Visual Studio Solution File
```

---

## 🔥 **How It Works**
### 1️⃣ **User Registration**
- A new user registers using **Username, Email, and Password**.
- The details are stored in **Firebase Realtime Database**.

### 2️⃣ **User Login**
- Users log in using **Email and Password**.
- Upon successful login, they are **redirected to the chat window**.

### 3️⃣ **Chat Window**
- **List of registered users** (except the logged-in user) is displayed.
- Clicking a **contact opens a chat window**.
- Messages are **stored and retrieved from Firebase in real-time**.

---

## 🛠️ **Troubleshooting**
### ❌ **Firebase Connection Issues?**
- Ensure your **Firebase Database URL** and **Auth Secret** are correct.
- Check if **Internet access** is available.

### ❌ **Messages Not Sending?**
- Ensure **Firebase Realtime Database Rules** allow reads/writes:
```json
{
  "rules": {
    ".read": true,
    ".write": true
  }
}
```

### ❌ **Contacts Not Loading?**
- Check if users exist in Firebase under `Users/`.
- Ensure you are **not filtering out the wrong users** in `FormChatWindow.cs`.

---

## 🤝 **Contributing**
Feel free to fork this repository and submit **pull requests**! 🚀

---

## 📜 **License**
This project is licensed under the **MIT License**.

🔗 **GitHub Repository:** [Chatify]

---

💡 **Have questions or suggestions?** Open an issue or reach out! 😊

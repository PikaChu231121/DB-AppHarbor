<template>
  <div>
    <div class="login-container">
      <div class="login-form">
        <h2>Login</h2>
        <form @submit.prevent="login">
          <div class="form-group">
            <label for="username">Username</label>
            <input type="text" id="username" v-model="username" required />
          </div>
          <div class="form-group">
            <label for="password">Password</label>
            <input type="password" id="password" v-model="password" required />
          </div>
          <button type="submit">Login</button>
        </form>
      </div>
    </div>

    <h1>Users</h1>
    <ul>
      <li v-for="user in users" :key="user.id">{{ user.nickname }}</li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      username: '',
      password: ''
    };
  },
  methods: {
    login() {
      const loginCredential = {
        username: this.username,
        password: this.password
      };
      
      axios.post("/api/Login", loginCredential)
        .then(response => {
          // Handle successful login
          console.log("Login successful", response.data);
          
        })
        .catch(error => {
          // Handle login error
          console.error("Login failed", error);
        });
    }
  }
};
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f5f5f5;
}

.login-form {
  background: white;
  padding: 2em;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  width: 300px;
}

.form-group {
  margin-bottom: 1em;
}

label {
  display: block;
  margin-bottom: 0.5em;
}

input {
  width: 100%;
  padding: 0.5em;
  border: 1px solid #ccc;
  border-radius: 4px;
}

button {
  width: 100%;
  padding: 0.5em;
  border: none;
  border-radius: 4px;
  background-color: #42b983;
  color: white;
  font-size: 1em;
  cursor: pointer;
}

button:hover {
  background-color: #349a72;
}
</style>

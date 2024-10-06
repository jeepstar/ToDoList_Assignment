// src/App.tsx
import React from 'react';
import { Provider } from 'react-redux';
import store from './redux/store';
import AddNewTodoItem from './components/AddNewTodoItem';
import TodoItems from './components/TodoItems';
import 'bootstrap/dist/css/bootstrap.min.css';

const App: React.FC = () => {
  return (
    <Provider store={store}>
      <div className="App">
        <h1>Todo List</h1>
        <AddNewTodoItem />
        <TodoItems />
      </div>
    </Provider>
  );
};

export default App;

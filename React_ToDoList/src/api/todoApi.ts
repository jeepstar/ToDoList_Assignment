// src/api/todoApi.ts
import axios from 'axios';
import { TodoItem, TodoItemDto } from '../models/todoItem';

const API_URL = 'https://localhost:44397/api/todoitems'; // backend URL

export const getTodoItems = async (): Promise<TodoItem[]> => {
    const response = await axios.get(API_URL);
    return response.data;
};

export const createTodoItem = async (todoItemDto: TodoItemDto): Promise<TodoItem> => {
    const response = await axios.post(API_URL, todoItemDto);
    return response.data;
};

export const markTodoItemAsComplete = async (id: string): Promise<void> => {
    await axios.put(`${API_URL}/${id}`, { isCompleted: true });
};

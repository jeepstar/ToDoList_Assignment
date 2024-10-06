// src/redux/todoSlice.ts
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { getTodoItems, createTodoItem, markTodoItemAsComplete } from '../api/todoApi';
import { TodoItem, TodoItemDto } from '../models/todoItem';

interface TodoState {
    items: TodoItem[];
    loading: boolean;
    error: string | null;
}

const initialState: TodoState = {
    items: [],
    loading: false,
    error: null,
};

export const fetchTodoItems = createAsyncThunk('todos/fetchTodoItems', async () => {
    const items = await getTodoItems();
    return items;
});

export const addTodoItem = createAsyncThunk('todos/addTodoItem', async (todoItemDto: TodoItemDto) => {
    const newItem = await createTodoItem(todoItemDto);
    return newItem;
});

export const completeTodoItem = createAsyncThunk('todos/completeTodoItem', async (id: string) => {
    await markTodoItemAsComplete(id);
    return id; // Return the ID to identify the item to update in the state
});

const todoSlice = createSlice({
    name: 'todos',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchTodoItems.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchTodoItems.fulfilled, (state, action) => {
                state.loading = false;
                state.items = action.payload;
            })
            .addCase(fetchTodoItems.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Failed to fetch todos';
            })
            .addCase(addTodoItem.fulfilled, (state, action) => {
                state.items.push(action.payload);
            })
            .addCase(completeTodoItem.fulfilled, (state, action) => {
                const id = action.payload;
                const existingItem = state.items.find((item) => item.id === id);
                if (existingItem) {
                    existingItem.isCompleted = true; // Update the completed status
                }
            });
    },
});

export default todoSlice.reducer;

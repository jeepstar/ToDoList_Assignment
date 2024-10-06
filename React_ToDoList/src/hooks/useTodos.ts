// src/hooks/useTodos.ts
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchTodoItems } from '../redux/todoSlice';
import { RootState } from '../redux/store';

const useTodos = () => {
  const dispatch = useDispatch();
  const todos = useSelector((state: RootState) => state.todos.items);
  const loading = useSelector((state: RootState) => state.todos.loading);
  const error = useSelector((state: RootState) => state.todos.error);

  useEffect(() => {
    dispatch(fetchTodoItems());
  }, [dispatch]);

  return { todos, loading, error };
};

export default useTodos;

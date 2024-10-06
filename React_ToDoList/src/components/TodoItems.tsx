// src/components/TodoItems.tsx
import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { completeTodoItem } from '../redux/todoSlice';
import { Table, Button } from 'react-bootstrap';
import { TodoItem } from '../models/todoItem';
import useTodos from '../hooks/useTodos';

const TodoItems: React.FC = () => {
    const { todos, loading, error } = useTodos();
    const dispatch = useDispatch();

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <Table striped bordered hover>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Description</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                {todos.map((item: TodoItem) => (
                    <tr key={item.id}>
                        <td>{item.id}</td>
                        <td>{item.description}</td>
                        <td>
                            {!item.isCompleted && (
                                <Button variant="warning" onClick={() => dispatch(completeTodoItem(item.id))}>
                                    Mark as Complete
                                </Button>
                            )}
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default TodoItems;

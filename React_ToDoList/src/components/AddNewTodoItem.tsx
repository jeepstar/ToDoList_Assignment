// src/components/AddNewTodoItem.tsx
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { addTodoItem } from '../redux/todoSlice';
import { TodoItemDto } from '../models/todoItem';
import { Button, Form, Row, Col } from 'react-bootstrap';

const AddNewTodoItem: React.FC = () => {
  const [description, setDescription] = useState('');
  const dispatch = useDispatch();

  const handleAdd = () => {
    const todoItemDto: TodoItemDto = { description };
    dispatch(addTodoItem(todoItemDto));
    setDescription(''); // Clear input after adding
  };

  return (
    <Form>
      <Row>
        <Col>
          <Form.Group controlId="formBasicEmail">
            <Form.Label>Description</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter todo item description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            />
          </Form.Group>
        </Col>
        <Col>
          <Button variant="primary" onClick={handleAdd}>
            Add Item
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default AddNewTodoItem;

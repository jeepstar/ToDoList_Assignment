// src/models/todoItem.ts
export interface TodoItem {
    id: string; // This should match the Guid type in C#
    description: string;
    isCompleted: boolean;
}

export interface TodoItemDto {
    description: string;
}

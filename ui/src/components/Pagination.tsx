import React from 'react';
import { Button, HStack } from '@chakra-ui/react';

interface PaginationProps {
  currentPage: number;
  onNextPageClick: () => void;
  onPreviousPageClick: () => void;
}

export const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  onNextPageClick,
  onPreviousPageClick,
}) => {
  return (
    <HStack spacing={2} mt={4} justifyContent={'center'}>
      <Button onClick={onPreviousPageClick} disabled={currentPage === 1}>
        Previous
      </Button>
      <Button onClick={onNextPageClick}>Next</Button>
    </HStack>
  );
};

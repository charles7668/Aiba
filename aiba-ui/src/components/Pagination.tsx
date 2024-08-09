import React from 'react';
import { Button, HStack, Select } from '@chakra-ui/react';

interface PaginationProps {
  currentPage: number;
  onNextPageClick: () => void;
  onPreviousPageClick: () => void;
  maxPage?: number;
  onTargetPageClick: (page: number) => () => void;
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

export const FixedCountPagination: React.FC<PaginationProps> = ({
  currentPage,
  maxPage,
  onNextPageClick,
  onPreviousPageClick,
  onTargetPageClick,
}) => {
  return (
    <HStack spacing={2} mt={4} justifyContent={'center'}>
      <Button onClick={onPreviousPageClick} disabled={currentPage < 2}>
        Previous
      </Button>
      <Select width={'xs'}>
        {Array.from({ length: maxPage ?? 0 }, (_, i) => (
          <option key={i + 1} value={i + 1} onClick={onTargetPageClick(i + 1)}>
            {i + 1}
          </option>
        ))}
      </Select>
      <Button
        onClick={onNextPageClick}
        disabled={maxPage === undefined ? true : currentPage > maxPage - 1}
      >
        Next
      </Button>
    </HStack>
  );
};

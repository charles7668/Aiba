import React, { useRef } from 'react';
import {
  AlertDialog,
  AlertDialogBody,
  AlertDialogContent,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogOverlay,
  Button,
} from '@chakra-ui/react';

export const SimpleAlertDialog: React.FC<{
  message: string;
  isOpen: boolean;
  setIsOpen: (open: boolean) => void;
  onConfirm?: () => void;
  onCancel?: () => void;
}> = ({ message, isOpen, setIsOpen, onConfirm, onCancel }) => {
  const cancelRef = useRef(null);

  const handleConfirm = () => {
    if (onConfirm) {
      onConfirm();
    }
    setIsOpen(false);
  };
  const handleOnClose = () => {
    if (onCancel) {
      onCancel();
    }
    setIsOpen(false);
  };
  return (
    <AlertDialog
      isCentered
      isOpen={isOpen}
      leastDestructiveRef={cancelRef}
      onClose={handleOnClose}
    >
      <AlertDialogOverlay>
        <AlertDialogContent>
          <AlertDialogHeader fontSize="md" fontWeight="bold">
            Confirm Action
          </AlertDialogHeader>
          <AlertDialogBody>{message}</AlertDialogBody>
          <AlertDialogFooter>
            <Button ref={cancelRef} onClick={handleOnClose}>
              Cancel
            </Button>
            <Button colorScheme="red" onClick={handleConfirm} ml={3}>
              Confirm
            </Button>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialogOverlay>
    </AlertDialog>
  );
};

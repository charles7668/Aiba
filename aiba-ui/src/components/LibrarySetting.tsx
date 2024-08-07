import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Heading,
  HStack,
  IconButton,
  Input,
  List,
  ListItem,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  Select,
  Text,
  useDisclosure,
} from '@chakra-ui/react';
import { MdAdd, MdDelete } from 'react-icons/md';
import { SimpleAlertDialog } from '../components/SimpleAlertDialog.tsx';
import { Api } from '../services/Api.ts';
import { MediaTypeFlag } from '../models/MediaTypeEnum.ts';
import { LibraryInfo } from '../models/LibraryInfo.ts';
import { FaPen } from 'react-icons/fa';

const LibraryList: React.FC<{
  libraries: Array<LibraryInfo>;
  onRemoveLibrary: (info: LibraryInfo) => Promise<void>;
}> = ({ libraries, onRemoveLibrary }) => {
  const [alertOpen, setAlertOpen] = useState(false);
  const [alertMessage, setAlertMessage] = useState('');
  const [alertConfirm, setAlertConfirm] = useState<() => void>(() => {});
  const onDelete = (info: LibraryInfo) => {
    setAlertMessage('Are you sure you want to delete ' + info.name + '?');
    setAlertConfirm(() => async () => {
      await onRemoveLibrary(info);
    });
    setAlertOpen(true);
  };

  return (
    <>
      <SimpleAlertDialog
        isOpen={alertOpen}
        setIsOpen={setAlertOpen}
        message={alertMessage}
        onConfirm={alertConfirm}
      />
      {libraries.map((library, index) => (
        <ListItem key={index} fontSize={'5xl'} width={'100%'}>
          <Box
            display={'flex'}
            borderWidth={'2px'}
            alignItems={'center'}
            justifyContent={'start'}
          >
            <Text as="span">{library.name}</Text>
            <IconButton
              icon={<FaPen />}
              aria-label={'edit'}
              variant={'outline'}
              color={'blue.500'}
              mr={5}
            ></IconButton>
            <IconButton
              variant={'outline'}
              color={'red'}
              icon={<MdDelete />}
              aria-label={'delete'}
              onClick={async () => {
                onDelete(library);
              }}
            ></IconButton>
          </Box>
        </ListItem>
      ))}
    </>
  );
};
const LibraryAddInputModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSubmit: ({
    name,
    path,
    type,
  }: {
    name: string;
    path: string;
    type: MediaTypeFlag;
  }) => Promise<{
    success: boolean;
    error: string;
  }>;
}> = ({ isOpen, onClose, onSubmit }) => {
  const [inputName, setInputName] = useState('');
  const [inputPath, setInputPath] = useState('');
  const [errorOpen, setErrorOpen] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [type, setType] = useState<MediaTypeFlag>(MediaTypeFlag.MANGA);

  const handleInputNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setInputName(e.target.value);
  };
  const handleInputPathChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setInputPath(e.target.value);
  };

  const handleSubmit = async () => {
    const result = await onSubmit({ name: inputName, path: inputPath, type });
    if (!result.success) {
      setErrorOpen(true);
      setErrorMessage(result.error);
      return;
    }
    setInputName('');
    setInputPath('');
    setType(MediaTypeFlag.MANGA);
    onClose();
  };

  return (
    <>
      <Modal isOpen={isOpen} onClose={onClose}>
        {SimpleAlertDialog({
          isOpen: errorOpen,
          setIsOpen: setErrorOpen,
          message: errorMessage,
        })}
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Add Library</ModalHeader>
          <ModalCloseButton />
          <ModalBody>
            <FormControl>
              <FormLabel>Name</FormLabel>
              <Input value={inputName} onChange={handleInputNameChange} />
            </FormControl>
            <FormControl>
              <FormLabel>Storage Path</FormLabel>
              <Input value={inputPath} onChange={handleInputPathChange} />
            </FormControl>
            <FormControl>
              <FormLabel>Media Type</FormLabel>
              <Select
                value={type}
                onChange={(e) =>
                  setType(Number(e.target.value) as MediaTypeFlag)
                }
              >
                {Object.keys(MediaTypeFlag)
                  .filter((key) => isNaN(Number(key)))
                  .map((key) => (
                    <option
                      key={key}
                      value={MediaTypeFlag[key as keyof typeof MediaTypeFlag]}
                    >
                      {key}
                    </option>
                  ))}
              </Select>
            </FormControl>
          </ModalBody>

          <ModalFooter>
            <Button colorScheme="blue" mr={3} onClick={handleSubmit}>
              Submit
            </Button>
            <Button variant="ghost" onClick={onClose}>
              Cancel
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
};

export const LibrarySetting: React.FC = () => {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const [libraries, setLibraries] = useState<Array<LibraryInfo>>([]);

  const onSubmit = async ({
    name,
    path,
    type,
  }: {
    name: string;
    path: string;
    type: MediaTypeFlag;
  }) => {
    const response = await Api.addLibrary({
      name: name,
      path: path,
      type: type,
    });
    if (response.status !== 200) {
      return {
        success: false,
        error: 'Failed to add library : ' + (await response.text()),
      };
    }
    setLibraries([...libraries, { name, path, type }]);
    return { success: true, error: '' };
  };
  const onRemoveLibrary = async (info: LibraryInfo) => {
    const response = await Api.deleteLibrary(info);
    if (response.status !== 200) {
      return;
    }
    setLibraries(libraries.filter((library) => library !== info));
  };

  useEffect(() => {
    Api.getLibraries().then(async (response) => {
      if (response.status !== 200) {
        return;
      }
      const libraries: Array<LibraryInfo> = await response.json();
      setLibraries(libraries);
    });
  }, []);

  return (
    <Box
      maxW="100%"
      borderWidth="1px"
      borderRadius="lg"
      overflow="hidden"
      p={4}
      bg="white"
      boxShadow="md"
      display={'flex'}
      justifyContent={'start'}
      flexDirection={'column'}
      alignItems={'start'}
    >
      <HStack
        alignItems={'center'}
        justifyContent={'center'}
        justifyItems={'center'}
        mb={4}
      >
        <Heading as="h2" size="lg" mr={4}>
          Libraries
        </Heading>
        <IconButton
          icon={<MdAdd />}
          aria-label="Add Library"
          onClick={() => onOpen()}
        />
        <LibraryAddInputModal
          isOpen={isOpen}
          onClose={onClose}
          onSubmit={onSubmit}
        ></LibraryAddInputModal>
      </HStack>
      <List spacing={3} width={'100%'}>
        {
          <LibraryList
            libraries={libraries}
            onRemoveLibrary={onRemoveLibrary}
          />
        }
      </List>
    </Box>
  );
};

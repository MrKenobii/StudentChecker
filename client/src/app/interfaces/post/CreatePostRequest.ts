export interface CreatePostRequest{
  title: string;
  content: string;
  recruiterId?: number | undefined | null;
  studentId?: number | undefined | null;
}
